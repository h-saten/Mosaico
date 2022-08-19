using FluentValidation;
using Mosaico.Cache.Redis.Configurations;

namespace Mosaico.Cache.Redis.Validators
{
    public class RedisCacheConfigurationValidator : AbstractValidator<RedisCacheConfiguration>
    {
        public RedisCacheConfigurationValidator()
        {
            RuleFor(c => c.RedisConnectionString).NotNull().WithMessage("Redis is misconfigured. Connection string cannot be empty in configuration");
            RuleFor(c => c.RedisDatabase).NotNull().WithMessage("Redis is misconfigured. Redis database cannot be empty in configuration.");
        }
    }
}