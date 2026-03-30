using Asp.Versioning;
using ECommerceAPI.Interfaces;
using ECommerceAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ECommerceAPI.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
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
        public IActionResult GetCart(int customerId)
        {
            List<CartItem> cartitem = _cartService.GetCart(customerId);
            return Ok(cartitem);
        }

        [HttpDelete()]
        public IActionResult RemoveFromCart(int cartItemId)
        {
            bool removed = _cartService.RemoveFromCart(cartItemId);
            return Ok("Product removed successfully");
        }
    }
    
}
