using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ECommerceAPI.Interfaces;
using ECommerceAPI.Models;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private IProductService _productService;

        public ProductController(IProductService productService)
        {
            this._productService = productService;
        }
        [HttpPost()]
        [Authorize(Roles = "Admin")]
        public IActionResult AddProduct(string name, double price, int stock, string category)
        {
            Product? product = _productService.AddProduct(name, price, stock, category);
            if (product != null)
            {
                return Ok($"Product Successfully Added, Details: {product}");
            }
            else
            {
                return Conflict("Product already exists");
            }
        }

        [HttpDelete()]
        [Authorize(Roles = "Admin")]
        public IActionResult RemoveProduct(int id)
        {
            bool removed = _productService.RemoveProduct(id);
            if (removed == true)
            {
                return NoContent();
            }
            else
            {
                return NotFound($"Product with id:{id} not found");
            }            
        }

        [HttpGet()]
        [Authorize]
        public IActionResult GetAllProducts()
        {
            List<Product> products = _productService.GetAllProducts();
            return Ok(products);
        }

        [HttpPatch()]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateStock(int id, int stock)
        {
            bool updated = _productService.UpdateStock(id, stock);
            if (updated == true)
            {
                return Ok("Product Stock Updated successfully");
            }
            else
            {
                return BadRequest("Product not found or stock below 0");
            }
        }
    }
}
