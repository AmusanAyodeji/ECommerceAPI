using Asp.Versioning;
using ECommerceAPI.DTOs.Products;
using ECommerceAPI.Interfaces;
using ECommerceAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Data.SqlClient;

namespace ECommerceAPI.Controllers.V2
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
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
        public IActionResult AddProduct(CreateProductV2DTO productDTO)
        {
            ProductResponseDTO? product = _productService.AddProductV2(productDTO);
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
            List<ProductResponseDTO> products = _productService.GetAllProducts();
            return Ok(products);
        }

        [HttpPatch()]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateStock(UpdateStockDTO updateStock)
        {
            bool updated = _productService.UpdateStock(updateStock);
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
