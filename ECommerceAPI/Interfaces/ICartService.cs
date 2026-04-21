using ECommerceAPI.DTOs.Cart;
using ECommerceAPI.Models;

namespace ECommerceAPI.Interfaces
{
    public interface ICartService
    {
        public bool AddToCart(CreateCartItemDTO cartDTO);
        public List<CartItemResponseDTO> GetCart(int customerId);
        public bool RemoveFromCart(int cartItemId);
        public void ClearCart(int customerId);
    }
}
