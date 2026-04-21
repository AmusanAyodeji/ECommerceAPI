namespace ECommerceAPI.DTOs.Cart
{
    public class CartItemDTO
    {
        public int customerId { get; set; }
        public int productId { get; set; }
        public int quantity { get; set; }
    }
    public class CartItemResponseDTO : CartItemDTO
    {
        public override string ToString()
        {
            return $"Customer ID: {customerId}, Product ID: {productId}, Quantity: {quantity}";
        }
    }
    public class CreateCartItemDTO : CartItemDTO
    {
    }
}
