using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ECommerceAPI.Interfaces;
using ECommerceAPI.Models;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            this._authService = authService;
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
                registered = false;
            }

            if(registered == true)
            {
                return Ok("Registration Successful");
            }
            else
            {
                return BadRequest("Registration Failed");
            }
        }

        [HttpPost("login")]
        public IActionResult Login(string username, string password)
        {
            User? user = _authService.Login(username, password);
            if(user != null)
            {
                return Ok(user);
            }
            else
            {
                return Unauthorized("Username or Password Incorrect");
            }
        }
    }
}
