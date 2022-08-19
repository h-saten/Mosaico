using Autofac;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Mosaico.Cache.Base;
using Mosaico.Cache.Base.Abstractions;
using Mosaico.Cache.Base.Pipelines;
using Mosaico.Cache.Redis.Configurations;
using Mosaico.Cache.Redis.Validators;

namespace Mosaico.Cache.Redis
{
    public class RedisCacheModule : Module
    {
        private readonly RedisCacheConfiguration _config;

        public RedisCacheModule(IConfiguration config)
        {
            _config = new RedisCacheConfiguration();
            config.GetSection(RedisCacheConfiguration.SectionName).Bind(_config);
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterInstance(_config).AsSelf();
            builder.RegisterType<RedisCacheConfigurationValidator>().As<IValidator<RedisCacheConfiguration>>();
            builder.RegisterGeneric(typeof(CachePipeline<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(CacheResetPipeline<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterType<RedisCacheClient>().As<ICacheClient>();
            builder.RegisterType<RedisSortedRepository>().As<ITimeSeriesRepository>();
        }
    }
}