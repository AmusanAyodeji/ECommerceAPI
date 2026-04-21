using ECommerceAPI.Models;

namespace ECommerceAPI.Interfaces
{
    public interface ICustomerService
    {
        public User GetById(int Id);
    }
}
