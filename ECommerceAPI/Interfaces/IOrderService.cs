using ECommerceAPI.Models;

namespace ECommerceAPI.Interfaces
{
    public interface IOrderService
    {
        public List<Order> GetCustomerOrders(int id);
        public bool CreateOrder(int customerId);
    }
}
