using ECommerceAPI.Models;
using ECommerceAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            if (added == true)
            {
                return Ok("Product added successfully");
            }
            return BadRequest("Error Adding Product");
        }

        [HttpGet()]
        public IActionResult GetCart(int customerId)
        {
            List<CartItem> cartitem = _cartService.GetCart(customerId);
            if(cartitem.Count > 0)
            {
                return Ok(cartitem);
            }
            return BadRequest("No Items for Customer");
        }

        [HttpDelete()]
        public IActionResult RemoveFromCart(int cartItemId)
        {
            bool removed = _cartService.RemoveFromCart(cartItemId);
            if (removed == true)
            {
                return Ok("Product removed successfully");
            }
            return BadRequest("Error removing Product");
        }
    }
    
}
