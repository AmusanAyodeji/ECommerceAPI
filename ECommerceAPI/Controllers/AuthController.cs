using ECommerceAPI.Interfaces;
using ECommerceAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Data.SqlClient;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    [EnableRateLimiting("auth")]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService;
        private ITokenService _tokenService;

        public AuthController(IAuthService authService, ITokenService tokenService)
        {
            this._authService = authService;
            this._tokenService = tokenService;
        }
        [HttpPost("register")]
        public IActionResult Register(string username, string password, string role)
        {
            bool registered;
            if (role.ToLower() == "admin")
            {
                registered = _authService.RegisterAdmin(username, password);
                
            }else if(role.ToLower() == "customer")
            {
                registered = _authService.RegisterCustomer(username, password);
            }
            else
            {
                return BadRequest("Invalid Role");
            }
            return Ok("Registration Successful");
        }

        [HttpPost("login")]
        public IActionResult Login(string username, string password)
        {
            User? user = _authService.Login(username, password);
            string token = _tokenService.GenerateToken(user);

            return Ok(new
            {
                Token = token,
                Username = user.UserName,
                Role = user.Role
            });
        }
    }
}
