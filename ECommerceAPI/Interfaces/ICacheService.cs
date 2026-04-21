namespace ECommerceAPI.Interfaces
{
    public interface ICacheService
    {
        public string? Get(string key);
        public void Set(string key, string value, TimeSpan? expiry = null);
        public bool Exists(string key);
        public bool Delete(string key);
    }
}
