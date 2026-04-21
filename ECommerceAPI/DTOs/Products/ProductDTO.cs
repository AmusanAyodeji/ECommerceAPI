namespace ECommerceAPI.DTOs.Products
{
    public class ProductDTO
    {
        public string name { get; set; }
        public double price { get; set; }
        public int stock { get; set; }
        
    }
    public class CreateProductDTO:ProductDTO
    {
        public string category { get; set; }
    }

    public class CreateProductV2DTO:ProductDTO
    {
        public int categoryId { get; set; }
    }
    public class ProductResponseDTO:ProductDTO
    {
        public string id { get; set; }
        public string? categoryId { get; set; }
        public string? category { get; set; }
        public override string ToString()
        {
            string message = "";
            if (categoryId == null)
            {
                message = $"Id: {id}, Name: {name}, Price: ${price}, Stock: {stock}, Category: {category}";
            }
            else
            {
                message = $"Id: {id}, Name: {name}, Price: ${price}, Stock: {stock}, CategoryId: {categoryId}";
            }
            return message;
        }
    }
}
