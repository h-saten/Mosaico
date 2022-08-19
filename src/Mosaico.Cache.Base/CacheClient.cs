using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Mosaico.Cache.Base.Abstractions;
using Newtonsoft.Json;

namespace Mosaico.Cache.Base
{
    public class CacheClient : ICacheClient
    {
        private readonly IDistributedCache _distributedCache;
        
        public CacheClient(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        
        public virtual Task CleanAsync(string key, CancellationToken token = new CancellationToken())
        {
            var keysToClean = new List<string>();
            if (!string.IsNullOrWhiteSpace(key))
            {
                key = key.ToLowerInvariant();
                if (key.Contains("*"))
                {
                    foreach (var lang in Mosaico.Base.Constants.Languages.All)
                    {
                        var pattern = key.Replace("*", lang);
                        keysToClean.Add(pattern);
                    }
                }
                else
                {
                    keysToClean.Add(key);
                }
            }

            return CleanAsync(keysToClean, token);
        }

        public virtual async Task CleanAsync(List<string> keys, CancellationToken token = new CancellationToken())
        {
            if (keys != null)
            {
                foreach (var key in keys)
                {
                    await _distributedCache.RemoveAsync(key, token);
                }
            }
        }

        public virtual async Task<TResponse> GetAsync<TResponse>(string key, CancellationToken token = new CancellationToken())
        {
            key = key.ToLowerInvariant();
            var redisPayload = await _distributedCache.GetAsync(key, token);
            if (redisPayload != null)
            {
                var serializedResponse = Encoding.UTF8.GetString(redisPayload);
                return (TResponse) JsonConvert.DeserializeObject(serializedResponse, typeof(TResponse));
            }

            return default;
        }

        public virtual async Task AddAsync<TResponse>(string key, TResponse body, int expiration = Constants.DefaultCacheExpiration, CancellationToken token = new CancellationToken())
        {
            if (body != null && !string.IsNullOrWhiteSpace(key))
            {
                key = key.ToLowerInvariant();
                
                if (expiration <= 0)
                {
                    expiration = Constants.DefaultCacheExpiration;
                }
                
                var serializedResponse = JsonConvert.SerializeObject(body);
                var redisPayload = Encoding.UTF8.GetBytes(serializedResponse);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTimeOffset.UtcNow.AddMinutes(expiration))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(Constants.DefaultSlidingExpiration));
                await _distributedCache.SetAsync(key, redisPayload, options, token);
            }
        }

        public virtual bool IsEnabled => true;
    }
}