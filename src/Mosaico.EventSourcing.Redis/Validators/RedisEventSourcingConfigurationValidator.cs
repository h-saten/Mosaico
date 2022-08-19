using FluentValidation;
using Mosaico.EventSourcing.Redis.Configurations;

namespace Mosaico.EventSourcing.Redis.Validators
{
    public class RedisEventSourcingConfigurationValidator : AbstractValidator<RedisEventSourcingConfiguration>
    {
        public RedisEventSourcingConfigurationValidator()
        {
            RuleFor(c => c.RedisDatabase).NotNull().WithMessage("Redis is misconfigured. Redis database cannot be empty in configuration.");
            RuleFor(c => c.StreamName).NotNull().WithMessage("Redis is misconfigured. Stream name cannot be empty in configuration");
            RuleFor(c => c.RedisConnectionString).NotNull().WithMessage("Redis is misconfigured. Connection string cannot be empty in configuration");
        }
    }
}