using ECommerceAPI.Models;

namespace ECommerceAPI.Interfaces
{
    public interface IAuthService
    {
        public bool RegisterCustomer(string username, string password);
        public bool RegisterAdmin(string username, string password);
        public User? Login(string username, string password);
    }
}
