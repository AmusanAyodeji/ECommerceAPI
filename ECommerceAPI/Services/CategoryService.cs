using ECommerceAPI.Data;
using ECommerceAPI.Interfaces;
using ECommerceAPI.Models;

namespace ECommerceAPI.Services
{
    public class CategoryService:ICategoryService
    {
        private AppDbContext db;
        public CategoryService(AppDbContext db)
        {
            this.db = db;
        }
        public List<Category> GetAllCategories()
        {
            return db.Categories.ToList();
        }
        public Category? AddCategory(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Name cannot be empty");
            }
            Category? db_category = db.Categories.FirstOrDefault(u => u.Name == name);
            if(db_category != null)
            {
                throw new InvalidOperationException("Name already exists");
            }
            Category category = new Category();
            category.Name = name;

            db.Categories.Add(category);
            db.SaveChanges();
            return category;
        }
       public bool RemoveCategory(int id)
        {
            Category? category= db.Categories.FirstOrDefault(u => u.Id == id);
            if(category == null)
            {
                throw new InvalidOperationException($"Category with id: {id} not found");
            }
            db.Categories.Remove(category);
            db.SaveChanges();
            return true;
        }
    }
}
