using Microsoft.Extensions.DependencyInjection;
using Mosaico.Cache.Redis.Configurations;

namespace Mosaico.Cache.Redis.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddRedisCache(this IServiceCollection services, RedisCacheConfiguration config)
        {
            services.AddStackExchangeRedisCache(options => options.Configuration = config.RedisConnectionString);
        }
    }
}