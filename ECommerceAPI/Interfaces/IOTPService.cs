using ECommerceAPI.DTOs.OTP;

namespace ECommerceAPI.Interfaces
{
    public interface IOTPService
    {
        public int GenerateOTP(string email);
        public bool VerifyOTP(VerifyOTPDTO verify);
    }
}
