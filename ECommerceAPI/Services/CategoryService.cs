using AutoMapper;
using ECommerceAPI.Data;
using ECommerceAPI.DTOs.Category;
using ECommerceAPI.Interfaces;
using ECommerceAPI.Models;

namespace ECommerceAPI.Services
{
    public class CategoryService:ICategoryService
    {
        private AppDbContext db;
        private ILogger<CategoryService> _logger;
        private IValidationHelper _validate;
        private IMapper mapper;
        public CategoryService(AppDbContext db, ILogger<CategoryService> _logger, IValidationHelper _validate, IMapper mapper)
        {
            this.db = db;
            this._logger = _logger;
            this._validate = _validate;
            this.mapper = mapper;
        }
        public List<CategoryResponseDTO> GetAllCategories()
        {
            _logger.LogInformation("Returned All Categories");
            List<CategoryResponseDTO> categorylist = mapper.Map<List<CategoryResponseDTO>>(db.Categories.ToList());
            return categorylist;
        }
        public CategoryResponseDTO? AddCategory(CreateCategoryDTO createCategoryDTO)
        {
            _validate.CheckIfEmpty(createCategoryDTO.name, "Name field cannot be empty");
            Category? db_category = db.Categories.FirstOrDefault(u => u.Name == createCategoryDTO.name);
            _validate.CheckIfNotNull(db_category, $"Name Already Exists, Name = {createCategoryDTO.name}");
            Category category = mapper.Map<Category>(createCategoryDTO);
            db.Categories.Add(category);
            db.SaveChanges();
            CategoryResponseDTO categoryResponse = mapper.Map<CategoryResponseDTO>(category);
            _logger.LogInformation("Added Category to Database, Name = {Name}", createCategoryDTO.name);
            return categoryResponse;
        }
       public bool RemoveCategory(int id)
        {
            Category? category= db.Categories.FirstOrDefault(u => u.Id == id);
            _validate.CheckIfNull(category, $"Category with id: {id} not found");
            db.Categories.Remove(category);
            db.SaveChanges();
            _logger.LogInformation("Category sucessfully removed");
            return true;
        }
    }
}
