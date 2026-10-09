using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;
using ShopSphere.Cache;
using System.Threading.Tasks;
using System;

namespace ShopSphere.Cache
{
    public class DistributedCacheService : ICacheService
    {
        private readonly IDistributedCache cache;
        private readonly ILogger<DistributedCacheService> logger;

        public DistributedCacheService(IDistributedCache cache, ILogger<DistributedCacheService> logger)
        {
            this.cache = cache;
            this.logger = logger;
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            try
            {
                var data = await cache.GetAsync(key);
                if (data == null || data.Length == 0)
                {
                    logger.LogDebug("Cache miss for key {Key}", key);
                    return default;
                }

                logger.LogDebug("Cache hit for key {Key} (size={Size} bytes)", key, data.Length);
                return JsonSerializer.Deserialize<T>(data);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Cache get failed for key {Key}", key);
                return default;
            }
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan ttl)
        {
            try
            {
                var bytes = JsonSerializer.SerializeToUtf8Bytes(value);
                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = ttl
                };
                await cache.SetAsync(key, bytes, options);
                logger.LogDebug("Cache set for key {Key} (ttl={Ttl}) bytes={Size}", key, ttl, bytes?.Length ?? 0);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Cache set failed for key {Key}", key);
            }
        }

        public async Task RemoveAsync(string key)
        {
            try
            {
                await cache.RemoveAsync(key);
                logger.LogDebug("Cache removed for key {Key}", key);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Cache remove failed for key {Key}", key);
            }
        }
    }
}
