using ECommerceAPI.DTOs.Auth;
using ECommerceAPI.Models;

namespace ECommerceAPI.Interfaces
{
    public interface IAuthService
    {
        public bool RegisterCustomer(RegisterUserDTO registerUserDTO);
        public bool RegisterAdmin(RegisterUserDTO registerUserDTO);
        public TokenResponse? Login(LoginUserDTO loginUserDTO);
        public void VerifyUser(string email);
    }
}
