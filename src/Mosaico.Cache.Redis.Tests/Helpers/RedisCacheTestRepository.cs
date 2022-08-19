using FluentValidation;
using Mosaico.Cache.Redis.Configurations;
using Mosaico.Infrastructure.Redis.Tests.Helpers;
using Serilog;

namespace Mosaico.Cache.Redis.Tests.Helpers
{
    public class RedisCacheTestRepository : RedisCacheRepositoryBase<TestCacheItem>
    {
        public RedisCacheTestRepository(RedisCacheConfiguration config, ILogger logger = null, IValidator<RedisCacheConfiguration> validator = null) : base(config, logger, validator)
        {
        }

        protected override string EntityType => Constants.RedisCacheTestTypeName;
    }
}