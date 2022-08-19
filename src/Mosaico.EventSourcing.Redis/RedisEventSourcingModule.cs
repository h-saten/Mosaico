using Autofac;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Mosaico.EventSourcing.Base;
using Mosaico.EventSourcing.Redis.Configurations;
using Mosaico.EventSourcing.Redis.Validators;

namespace Mosaico.EventSourcing.Redis
{
    public class RedisEventSourcingModule : Module
    {
        private readonly RedisEventSourcingConfiguration _config;

        public RedisEventSourcingModule(IConfiguration configuration)
        {
            _config = new RedisEventSourcingConfiguration();
            configuration.GetSection(RedisEventSourcingConfiguration.SectionName).Bind(_config);
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterInstance(_config);
            builder.RegisterType<RedisEventRepository>().As<IEventRepository>();
            builder.RegisterType<RedisEventSourcingConfigurationValidator>().As<IValidator<RedisEventSourcingConfiguration>>();
            builder.RegisterType<DefaultSystemEventFactory>().As<ISystemEventFactory>();
        }
    }
}