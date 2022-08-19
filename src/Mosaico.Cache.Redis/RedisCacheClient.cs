using Microsoft.Extensions.Caching.Distributed;
using Mosaico.Cache.Base;
using Mosaico.Cache.Redis.Configurations;

namespace Mosaico.Cache.Redis
{
    public class RedisCacheClient : CacheClient
    {
        private readonly RedisCacheConfiguration _cacheConfiguration;
        public RedisCacheClient(IDistributedCache distributedCache, RedisCacheConfiguration cacheConfiguration) : base(distributedCache)
        {
            _cacheConfiguration = cacheConfiguration;
        }

        public override bool IsEnabled => _cacheConfiguration.IsEnabled;
    }
}