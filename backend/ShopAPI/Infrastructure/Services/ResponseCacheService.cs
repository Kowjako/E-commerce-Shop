using Core.Interface;
using StackExchange.Redis;
using System.Text.Json;

namespace Infrastructure.Services
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IDatabase _redis;

        public ResponseCacheService(IConnectionMultiplexer redis)
        {
            _redis = redis.GetDatabase();
        }

        public async Task CacheResponseAsync(string cacheKey, object response, TimeSpan duration)
        {
            if (response == null) return;

            var options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var serializedResponse = JsonSerializer.Serialize(response, options);
            await _redis.StringSetAsync(cacheKey, serializedResponse, duration);
        }

        public async Task<string> GetCachcedResponseAsync(string cacheKey)
        {
            var cachedResponse = await _redis.StringGetAsync(cacheKey);
            if (string.IsNullOrEmpty(cachedResponse)) return null;
            return cachedResponse;
        }
    }
}
