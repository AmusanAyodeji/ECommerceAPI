namespace ECommerceAPI.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Product> Products { get; set; }

        public override string ToString()
        {
            return $"Category Id: {Id}, Category Name: {Name}";
        }
    }
}