using Asp.Versioning;
using ECommerceAPI.Data;
using ECommerceAPI.DTOs.Category;
using ECommerceAPI.Interfaces;
using ECommerceAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.Controllers.V2
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CategoryController : ControllerBase
    {
        private ICategoryService _categoryservice;
        public CategoryController(ICategoryService _categoryService)
        {
            this._categoryservice = _categoryService;
        }
        [HttpGet()]
        public IActionResult GetAllCategories()
        {
            return Ok(_categoryservice.GetAllCategories());
        }

        [HttpPost]
        public IActionResult AddCategory(CreateCategoryDTO categoryDTO)
        {
            CategoryResponseDTO? category = _categoryservice.AddCategory(categoryDTO);
            if (category != null)
            {
                return Ok($"Category Successfully Added, Details: {category}");
            }
            else
            {
                return Conflict("Category already exists");
            }
        }

        [HttpDelete]
        public IActionResult RemoveCategory(int id)
        {
            bool removed = _categoryservice.RemoveCategory(id);
            if (removed == true)
            {
                return NoContent();
            }
            else
            {
                return NotFound($"Category with id:{id} not found");
            }
        }
    }
}
