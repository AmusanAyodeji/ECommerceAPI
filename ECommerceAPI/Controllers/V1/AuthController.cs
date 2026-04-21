using Asp.Versioning;
using ECommerceAPI.DTOs.Auth;
using ECommerceAPI.DTOs.OTP;
using ECommerceAPI.Interfaces;
using ECommerceAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace ECommerceAPI.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [AllowAnonymous]
    [EnableRateLimiting("auth")]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService;
        private ITokenService _tokenService;
        private IEmailService _emailservice;
        private IOTPService _otpservice;

        public AuthController(IAuthService authService, ITokenService tokenService, IEmailService _emailservice, IOTPService _otpservice)
        {
            this._authService = authService;
            this._tokenService = tokenService;
            this._emailservice = _emailservice;
            this._otpservice = _otpservice;
        }
        [HttpPost("register")]
        public IActionResult Register(RegisterUserDTO registerUserDTO)
        {
            if (registerUserDTO.role.ToLower() == "admin")
            {
                _authService.RegisterAdmin(registerUserDTO);
                
            }else if(registerUserDTO.role.ToLower() == "customer")
            {
                _authService.RegisterCustomer(registerUserDTO);
            }
            else
            {
                return BadRequest("Invalid Role");
            }
            return Ok("Registration Successful");
        }

        [HttpPost("login")]
        public IActionResult Login(LoginUserDTO loginUserDTO)
        {
            TokenResponse? token = _authService.Login(loginUserDTO);
            return Ok(token);
        }

        [HttpPost("refresh")]
        public IActionResult Refresh(int userId)
        {
            if (userId == 0)
                throw new ArgumentNullException("User Id",
                    "User Id cannot be empty");

            TokenResponse tokens = _tokenService
                .RefreshTokens(userId);

            return Ok(tokens);
        }

        [HttpPost("sendotp")]
        [EnableRateLimiting("otp")]
        public IActionResult SendOTP(string email)
        {
            _emailservice.SendOtp(email, _otpservice.GenerateOTP(email));
            return Ok("OTP Email sent successfully");
        }

        [HttpPost("verifyotp")]
        [EnableRateLimiting("otp")]
        public IActionResult VerifyOTP(VerifyOTPDTO verify)
        {
            if(_otpservice.VerifyOTP(verify))
            {
                _authService.VerifyUser(verify.email);
                return Ok("Email successfully verified");
            }
            return Unauthorized("Invalid OTP Code");
        }

        [HttpPost("resendotp")]
        [EnableRateLimiting("email")]
        public IActionResult ResendOTPEmail(string email)
        {
            _emailservice.ResendOTP(email);
            return Ok("OTP Email resent successfully");
        }
    }
}
