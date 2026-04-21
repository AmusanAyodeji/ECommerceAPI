using ECommerceAPI.Interfaces;
using ECommerceAPI.Settings;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace ECommerceAPI.Services
{
    public class RedisCacheService: ICacheService
    {
        private readonly IConnectionMultiplexer redis;
        private readonly IDatabase db;
        private readonly RedisSettings settings;
        private readonly ILogger<RedisCacheService> _logger;

        public RedisCacheService(IConnectionMultiplexer redis, IOptions<RedisSettings> settings, ILogger<RedisCacheService> logger)
        {
            this.redis = redis;
            this.db = redis.GetDatabase();
            this.settings = settings.Value;
            this._logger = logger;
        }
        public string? Get(string key)
        {
            string value = db.StringGet($"{key}");
            if (string.IsNullOrEmpty(value))
            {
                _logger.LogInformation("Cache MISS: {Key}", key);
                return default;
            }
            _logger.LogInformation("Cache HIT: {Key}", key);
            return value;
        }
        public void Set(string key, string value, TimeSpan? expiry = null)
        {
            TimeSpan cacheExpiry = expiry ?? TimeSpan.FromMinutes(settings.DefaultExpiryMinutes);
            db.StringSet(
                    $"{key}",
                    value,
                    cacheExpiry);
            _logger.LogInformation("Cache SET: {Key} expires in {Expiry}", key, cacheExpiry);
        }
        public bool Exists(string key)
        {
            if (db.KeyExists($"{key}")){
                _logger.LogInformation("Key: {Key} exists", key);
                return true;
            }
            _logger.LogInformation("Key: {Key} doesnt exist", key);
            return false;
        }
        public bool Delete(string key)
        {
            if (db.KeyDelete($"{key}"))
            {
                _logger.LogInformation("Key: {Key} deleted successfully", key);
                return true;
            }
            _logger.LogInformation("Error deleting Key: {Key}", key);
            return false;
        }

    }
}
