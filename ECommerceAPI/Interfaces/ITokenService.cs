using ECommerceAPI.Models;

namespace ECommerceAPI.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
