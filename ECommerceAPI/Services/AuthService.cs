using ECommerceAPI.Data;
using System;
using System.Collections.Generic;
using System.Text;
using ECommerceAPI.Enums;
using ECommerceAPI.Models;
using ECommerceAPI.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Identity;

namespace ECommerceAPI.Services
{
    public class AuthService:IAuthService
    {
        private AppDbContext db;
        private readonly IPasswordHasher<User> _hasher;

        public AuthService(AppDbContext db, IPasswordHasher<User> hasher)
        {
            this.db = db;
            this._hasher = hasher;
        }
        public bool RegisterCustomer(string username, string password)
        {
            if (username == null || password == null)
            {
                throw new ArgumentNullException("Invaid Username or Password");
            }
            Customer customer = new Customer();
            customer.UserName = username;

            customer.Password = _hasher.HashPassword(customer, password);

            customer.Role = Roles.Customer;
            User? db_user = db.Users.FirstOrDefault(u => u.UserName == username);
            if (db_user != null)
            {
                throw new InvalidOperationException("Username taken");
            }
            else
            {
                db.Users.Add(customer);
                db.SaveChanges();
                return true;
            }
        }

        public bool RegisterAdmin(string username, string password)
        {
            if (username == null || password == null)
            {
                throw new ArgumentNullException("Invaid Username or Password");
            }
            Admin admin = new Admin();
            admin.UserName = username;

            admin.Password = _hasher.HashPassword(admin, password);

            admin.Role = Roles.Admin;

            User? db_user = db.Users.FirstOrDefault(u => u.UserName == username);
            if (db_user != null)
            {
                throw new InvalidOperationException("Username taken");
            }
            else
            {
                db.Users.Add(admin);
                db.SaveChanges();
                return true;
            }
        }
        public User? Login(string username, string password)
        {
            
            User? db_user = db.Users.FirstOrDefault(u => u.UserName == username);
            if (db_user == null)
            {
                throw new UnauthorizedAccessException("Username or Password Incorrect");
            }

            var result = _hasher.VerifyHashedPassword(db_user, db_user.Password, password);

            if (result == PasswordVerificationResult.Success){
                return db_user;
            }
            else
            {
                throw new UnauthorizedAccessException("Username or Password Incorrect");
            }
            
        } 
    }
}
