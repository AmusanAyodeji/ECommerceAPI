using ECommerceAPI.Models;
using ECommerceAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private ICartService _cartService;

        public CartController(ICartService cartService)
        {
            this._cartService = cartService;
        }
        [HttpPost()]
        public IActionResult AddToCart(int customerId, int productId, int quantity)
        {
            bool added = _cartService.AddToCart(customerId, productId, quantity);
            return Ok("Product added successfully");
        }

        [HttpGet()]
        [Authorize]
        public IActionResult GetCart(int customerId)
        {
            List<CartItem> cartitem = _cartService.GetCart(customerId);
            return Ok(cartitem);
        }

        [HttpDelete()]
        [Authorize]
        public IActionResult RemoveFromCart(int cartItemId)
        {
            bool removed = _cartService.RemoveFromCart(cartItemId);
            return Ok("Product removed successfully");
        }
    }
    
}
