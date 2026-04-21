namespace ECommerceAPI.DTOs.Auth
{
    public class UserDTO
    {
        public string username { get; set; }
        public string password { get; set; }
    }
    public class RegisterUserDTO : UserDTO
    {
        public string role { get; set; }
    }
    public class LoginUserDTO : UserDTO
    {
    }
}

