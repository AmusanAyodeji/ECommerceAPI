using ECommerceAPI.Data;
using ECommerceAPI.DTOs.OTP;
using ECommerceAPI.Interfaces;
using ECommerceAPI.Models;

namespace ECommerceAPI.Services
{
    public class OTPService:IOTPService
    {
        private ICacheService redis;
        private AppDbContext db;
        private ILogger<OTPService> _logger;

        public OTPService(AppDbContext db, ILogger<OTPService> _logger, ICacheService redis)
        {
            this.db = db;
            this._logger = _logger;
            this.redis = redis;
        }

        public int GenerateOTP(string email)
        {
            Random rand = new Random();
            int code = rand.Next(1000, 9999);
            redis.Delete($"otp:{email}");
            redis.Set($"otp:{email}", $"{code}", TimeSpan.FromMinutes(5));
            return code;
        }

        public bool VerifyOTP(VerifyOTPDTO verify)
        {
            string? cache_otp = redis.Get($"otp:{verify.email}");
            if(cache_otp == null)
            {
                _logger.LogWarning("Email not found");
                throw new UnauthorizedAccessException("Email not found");
            }
            if(verify.otp == int.Parse(cache_otp))
            {
                redis.Delete($"otp:{verify.email}");
                User db_user = new User();
                db_user.UserName = verify.email;
                db.Users.Add(db_user);
                db.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
