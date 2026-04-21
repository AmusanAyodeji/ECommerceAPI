using ECommerceAPI.Models;
using ECommerceAPI.DTOs.Category;


namespace ECommerceAPI.Interfaces
{
    public interface ICategoryService
    {
        public List<CategoryResponseDTO> GetAllCategories();
        public CategoryResponseDTO? AddCategory(CreateCategoryDTO createCategoryDTO);
        public bool RemoveCategory(int id);
    }
}
