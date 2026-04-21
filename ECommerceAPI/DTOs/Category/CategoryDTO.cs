namespace ECommerceAPI.DTOs.Category
{
    public class CategoryDTO
    {
        public string name { get; set; }
    }

    public class CategoryResponseDTO: CategoryDTO
    {
        public override string ToString()
        {
            return $"Name: {name}";
        }
    }

    public class CreateCategoryDTO: CategoryDTO
    {
    }

}
    
