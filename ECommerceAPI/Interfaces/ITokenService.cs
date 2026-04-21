using ECommerceAPI.Models;

namespace ECommerceAPI.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
        string GenerateRefreshToken();
        void SaveRefreshToken(int userId, string refreshToken);
        TokenResponse RefreshTokens(int userId);
    }
}
