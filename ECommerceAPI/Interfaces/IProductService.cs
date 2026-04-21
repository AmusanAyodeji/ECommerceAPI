using ECommerceAPI.DTOs.Products;
using ECommerceAPI.Models;

namespace ECommerceAPI.Interfaces
{
    public interface IProductService
    {
        public List<ProductResponseDTO> GetAllProducts();
        public ProductResponseDTO? AddProduct(CreateProductDTO productDTO);
        public ProductResponseDTO? AddProductV2(CreateProductV2DTO productDTO);

        public bool RemoveProduct(int id);
        public bool UpdateStock(UpdateStockDTO updateStock);
        public Product GetById(int Id);
        public void UpdateStockFromCart(CartItem item);
        public double GetTotalPrice(List<CartItem> cartitems);
    }
}
