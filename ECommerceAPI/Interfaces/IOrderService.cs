using ECommerceAPI.DTOs.Orders;
using ECommerceAPI.Models;

namespace ECommerceAPI.Interfaces
{
    public interface IOrderService
    {
        public List<OrderResponseDTO> GetCustomerOrders(int id);
        public bool CreateOrder(int customerId);
    }
}
