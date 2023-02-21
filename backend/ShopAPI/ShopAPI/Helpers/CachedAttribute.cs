using Core.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

namespace ShopAPI.Helpers
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveSeconds;

        public CachedAttribute(int timeToLiveSeconds)
        {
            _timeToLiveSeconds = timeToLiveSeconds;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var service = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
            var key = GenerateCacheKeyFromRequest(context.HttpContext.Request);

            var cachedResponse = await service.GetCachcedResponseAsync(key);

            if (!string.IsNullOrEmpty(cachedResponse))
            {
                var contentResult = new OkObjectResult(cachedResponse);
                context.Result = contentResult;
                return;
            }

            var executedContext = await next(); //move to controller
            if(executedContext.Result is OkObjectResult result)
            {
                await service.CacheResponseAsync(key, 
                                                 result.Value, 
                                                 TimeSpan.FromSeconds(_timeToLiveSeconds));
            }

        }

        private string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();
            keyBuilder.Append($"{request.Path}");

            foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
            {
                keyBuilder.Append($"|{key}-{value}");
            }

            return keyBuilder.ToString();
        }
    }
}
