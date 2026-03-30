using ECommerceAPI.Models;

namespace ECommerceAPI.Interfaces
{
    public interface IProductService
    {
        public List<Product> GetAllProducts();
        public Product? AddProduct(string name, double price, int stock, string category);
        public Product? AddProductV2(string name, double price, int stock, int categoryId);

        public bool RemoveProduct(int id);
        public bool UpdateStock(int id, int stock);
    }
}
