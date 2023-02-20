namespace Core.Interface
{
    public interface IResponseCacheService
    {
        Task CacheResponseAsync(string cacheKey, object response, TimeSpan duration);
        Task<string> GetCachcedResponseAsync(string cacheKey);
    }
}
