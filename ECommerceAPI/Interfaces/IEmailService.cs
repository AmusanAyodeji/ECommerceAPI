namespace ECommerceAPI.Interfaces
{
    public interface IEmailService
    {
        public Task SendOtp(string email, int otp);
        public Task ResendOTP(string email);
    }
}
