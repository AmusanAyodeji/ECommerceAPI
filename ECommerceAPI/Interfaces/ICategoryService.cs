using ECommerceAPI.Models;

namespace ECommerceAPI.Interfaces
{
    public interface ICategoryService
    {
        public List<Category> GetAllCategories();
        public Category? AddCategory(string name);
        public bool RemoveCategory(int id);
    }
}
