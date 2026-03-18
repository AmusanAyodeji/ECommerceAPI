using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ECommerceAPI.Interfaces;
using ECommerceAPI.Models;

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
        [HttpPost("add_product")]
        public IActionResult AddProduct(string name, double price, int stock, string category)
        {
            Product? product = _productService.AddProduct(name, price, stock, category);
            if(product != null)
            {
                return Ok($"Product Successfully Added, Details: {product}");
            }
            else
            {
                return BadRequest("Unable to Add Product");
            }
        }

        [HttpDelete("remove_product")]
        public IActionResult RemoveProduct(int id)
        {
            bool removed = _productService.RemoveProduct(id);
            if(removed == true)
            {
                return Ok("Product deleted successfully");
            }
            else
            {
                return BadRequest("Product not found");
            }
        }

        [HttpGet("products")]
        public IActionResult GetAllProducts()
        {
            List<Product> products = _productService.GetAllProducts();
            return Ok(products);
        }

        [HttpPatch("update_stock")]
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
