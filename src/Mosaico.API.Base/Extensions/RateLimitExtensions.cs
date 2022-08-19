using AspNetCoreRateLimit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mosaico.API.Base.Configurations;
using System;

namespace Mosaico.API.Base.Extensions
{
    public static class RateLimitExtensions
    {
        public static void AddRateLimit(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache();

            // inject counter and rules stores
            services.AddInMemoryRateLimiting();

            // configuration (resolvers, counter key builders)
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.Configure<IpRateLimitOptions>(configuration.GetSection(RateLimitConfigurations.SectionName));

        }
    }
}