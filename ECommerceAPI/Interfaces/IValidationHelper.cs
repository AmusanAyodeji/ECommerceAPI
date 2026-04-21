namespace ECommerceAPI.Interfaces
{
    public interface IValidationHelper
    {
        public void CheckIfEmpty(string property, string message);
        public void CheckIfNotNull<T>(T property, string message);
        public void CheckIfNull<T>(T property, string message);
        public void CheckLessThanZero(double property, string message);
        public void CheckLessEqualZero(int property, string message);
        public void CheckEqualZero(int property, string message);
        public void ValidateAmount(double amount);

    }
}
