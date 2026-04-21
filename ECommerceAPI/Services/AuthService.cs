using AutoMapper;
using Azure.Core;
using ECommerceAPI.Data;
using ECommerceAPI.DTOs.Auth;
using ECommerceAPI.Enums;
using ECommerceAPI.Interfaces;
using ECommerceAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace ECommerceAPI.Services
{
    public class AuthService:IAuthService
    {
        private AppDbContext db;
        private IPasswordHasher<User> _hasher;
        private ILogger<AuthService> _logger;
        private ITokenService _tokenservice;
        private readonly IConfiguration config;
        private IWalletService _walletService;
        public AuthService(AppDbContext db, IPasswordHasher<User> hasher, ILogger<AuthService> logger, ITokenService _tokenservice, IConfiguration config, IWalletService _walletService)
        {
            this.db = db;
            this._hasher = hasher;
            this._logger = logger;
            this._tokenservice = _tokenservice;
            this.config = config;
            this._walletService = _walletService;
        }

        private void ValidatePassword(string password)
        {
            if(password.Length < 8)
            {
                throw new ArgumentException("Password should be 8 or more characters");
            }
            bool hasSpecialChar = password.Any(ch => !char.IsLetterOrDigit(ch));
            if (!hasSpecialChar)
            {
                throw new ArgumentException("Password must include a special character");
            }
            bool hasNumber = password.Any(ch => char.IsDigit(ch));
            if (!hasNumber)
            {
                throw new ArgumentException("Password must include a number");
            }
            bool hasLetter = password.Any(ch => char.IsDigit(ch));
            if (!hasLetter)
            {
                throw new ArgumentException("Password must include a letter");
            }
            bool hasCap = password.Any(ch => char.IsUpper(ch));
            if (!hasCap)
            {
                throw new ArgumentException("Password must include a capital letter");
            }
        }
        private static void ValidateEmail(string email)
        {
            var addr = new MailAddress(email);
            if(!(addr.Address == email))
            {
                throw new ArgumentException("Invalid Email");
            }
        }

        private void ValidateUser(string username, string password)
        {
            if (username == null || password == null)
            {
                _logger.LogWarning("Registration failed due to null username or password");
                throw new ArgumentNullException("Invaid Username or Password");
            }
            ValidateEmail(username);
            ValidatePassword(password);
            User? db_user = db.Users.FirstOrDefault(u => u.UserName == username);
            if (db_user != null && !db_user.IsVerified)
            {
                _logger.LogWarning("Registration failed. Username already taken, Attempted Username = {AttemptedUsername}", username);
                throw new InvalidOperationException("Username taken");
            }
        }

        public bool RegisterCustomer(RegisterUserDTO userDTO)
        {
            ValidateUser(userDTO.username, userDTO.password);
            User? customer = db.Users.FirstOrDefault(u => u.UserName == userDTO.username);
            if (customer == null || (customer.UserName == userDTO.username && customer.IsVerified == false))
            {
                _logger.LogWarning("User Account not Verified Username = {Username}", userDTO.username);
                throw new UnauthorizedAccessException("User Not Verified");
            }
            customer.Password = _hasher.HashPassword(customer, userDTO.password);
            customer.Role = Roles.Customer;
            db.SaveChanges();
            _walletService.CreateWallet(customer.Id);
            return true;
        }

        public bool RegisterAdmin(RegisterUserDTO userDTO)
        {
            ValidateUser(userDTO.username, userDTO.password);
            User? admin = db.Users.FirstOrDefault(u => u.UserName == userDTO.username);
            if (admin == null || (admin.UserName == userDTO.username && admin.Password == null)) {
                _logger.LogWarning("User Account not Verified Username = {Username}", userDTO.username);
                throw new UnauthorizedAccessException("User Not Verified");
            }
            admin.Password = _hasher.HashPassword(admin, userDTO.password);
            admin.Role = Roles.Admin;
            db.SaveChanges();
            _walletService.CreateWallet(admin.Id);
            return true;
        }
        public TokenResponse? Login(LoginUserDTO loginDTO)
        {

            User? db_user = db.Users.FirstOrDefault(u => u.UserName == loginDTO.username);
            if (db_user == null)
            {
                _logger.LogWarning("Login Failed. Attempted Username = {Username} does not exist", loginDTO.username);
                throw new UnauthorizedAccessException("Username or Password Incorrect");
            }

            if (!db_user.IsVerified){
                _logger.LogWarning("User Account not Verified Username = {Username}", loginDTO.username);
                throw new UnauthorizedAccessException("User Not Verified");
            }            

            var result = _hasher.VerifyHashedPassword(db_user, db_user.Password, loginDTO.password);

            if (result == PasswordVerificationResult.Success){
                string accesstoken = _tokenservice.GenerateToken(db_user);
                string refreshtoken = _tokenservice.GenerateRefreshToken();
                _tokenservice.SaveRefreshToken(db_user.Id, refreshtoken);

                _logger.LogInformation("Login Successful. Role = {Role}",db_user.Role.ToString());
                TokenResponse tokenresponse = new TokenResponse
                {
                    AccessToken = accesstoken,
                    RefreshToken = refreshtoken,
                    AccessTokenExpiry = DateTime.UtcNow.AddMinutes(double.Parse(config["JwtSettings:AccessTokenExpiryMinutes"])),
                    RefreshTokenExpiry = DateTime.UtcNow.AddDays(double.Parse(config["JwtSettings:RefreshTokenExpiryDays"])),
                    Username = loginDTO.username,
                    Role = db_user.Role.ToString()
                };
                return tokenresponse;
            }
            else
            {
                _logger.LogWarning("Login Failed, Incorrect Password. Attempted Username = {Username}", loginDTO.username);
                throw new UnauthorizedAccessException("Username or Password Incorrect");
            }            
        } 
        public void VerifyUser(string email)
        {
            User? db_user = db.Users.FirstOrDefault(u => u.UserName == email);
            if(db_user == null)
            {
                _logger.LogWarning("Email = {email} does not exist", email);
                throw new UnauthorizedAccessException("Email does not exist");
            }
            db_user.IsVerified = true;
            db.SaveChanges();
        }
    }
}
