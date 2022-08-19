using FluentValidation;
using Mosaico.Cache.Redis;
using Mosaico.Cache.Redis.Configurations;
using Serilog;

namespace Mosaico.Application.Wallet.Caching
{
    public class CompanyBalanceCache : RedisCacheRepositoryBase<CompanyNativeBalanceCacheItem>
    {
        public CompanyBalanceCache(RedisCacheConfiguration config, ILogger logger = null, IValidator<RedisCacheConfiguration> validator = null) : base(config, logger, validator)
        {
        }
    }
}