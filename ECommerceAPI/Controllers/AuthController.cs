using ECommerceAPI.Interfaces;
using ECommerceAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

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
                return BadRequest("Invalid Role");
            }
            return Ok("Registration Successful");
        }

        [HttpPost("login")]
        public IActionResult Login(string username, string password)
        {
            User? user = _authService.Login(username, password);
            return Ok(user);
        }
    }
}
