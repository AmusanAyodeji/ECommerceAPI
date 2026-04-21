using ECommerceAPI.Interfaces;
using ECommerceAPI.Models;
using ECommerceAPI.Services;

namespace ECommerceAPI.Helper
{
    public class ValidationHelper: IValidationHelper
    {
        private ILogger<ValidationHelper> _logger;

        public ValidationHelper(ILogger<ValidationHelper> _logger)
        {
            this._logger = _logger;
        }
        public void CheckIfEmpty(string property, string message)
        {
            if (string.IsNullOrEmpty(property))
            {
                _logger.LogWarning(message);
                throw new ArgumentNullException(message);
            }
        }

        public void CheckEqualZero(int property, string message)
        {
            if (property == 0)
            {
                _logger.LogWarning(message);
                throw new ArgumentNullException(message);
            }
        }

        public void CheckIfNotNull<T>(T property, string message)
        {
            if (property != null)
            {
                _logger.LogWarning(message);
                throw new ArgumentException(message);
            }
        }

        public void CheckIfNull<T>(T property, string message)
        {
            if (property == null)
            {
                _logger.LogWarning(message);
                throw new InvalidOperationException(message);
            }
        }

        public void CheckLessEqualZero(int property, string message)
        {
            if (property <= 0)
            {
                _logger.LogWarning(message);
                throw new ArgumentException(message);
            }
        }

        public void CheckLessThanZero(double property, string message)
        {
            if (property < 0)
            {
                _logger.LogWarning(message);
                throw new ArgumentException(message);
            }
        }
        public void ValidateAmount(double amount)
        {
            if (amount < 200)
            {
                throw new ArgumentException("Minimum amount is 200");
            }
        }
    }
}
