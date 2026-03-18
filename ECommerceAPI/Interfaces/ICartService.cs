using ECommerceAPI.Models;

namespace ECommerceAPI.Interfaces
{
    public interface ICartService
    {
        public bool AddToCart(int customerId, int productId, int quantity);
        public List<CartItem> GetCart(int customerId);
        public bool RemoveFromCart(int cartItemId);
    }
}
