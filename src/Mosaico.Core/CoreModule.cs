using System;
using System.Collections.Generic;
using Autofac;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Mosaico.Base;
using Mosaico.Base.Abstractions;
using Mosaico.Base.Tools;
using Mosaico.Cache.Redis;
using Mosaico.Core.EntityFramework.Abstractions;
using Mosaico.Core.EntityFramework.Interceptors;
using Mosaico.Core.Settings;
using Mosaico.CQRS.Base;
using Mosaico.Events.Base;
using Mosaico.Events.RabbitMq;
using Mosaico.Events.ServiceBus;
using Mosaico.EventSourcing.Redis;
using Mosaico.Integration.Blockchain.Ethereum;
using Mosaico.Payments.Binance;
using Mosaico.Payments.RampNetwork;
using Mosaico.Payments.Transak;
using Mosaico.Storage.AzureBlobStorage;
using Module = Autofac.Module;

namespace Mosaico.Core
{
    /*
     * CoreModule contains basic dependency registrations like TemplateEngine, Automapper, PubSub, Email API integration
     */
    public class CoreModule : Module
    {
        private readonly IConfiguration _configuration;
        private readonly List<Type> _mappingProfileTypes;

        public CoreModule(List<Type> mappingProfileTypes, IConfiguration configuration)
        {
            _mappingProfileTypes = mappingProfileTypes;
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            
            //configuration
            var coreSettings = new CoreSettings();
            _configuration.GetSection(CoreSettings.SectionName).Bind(coreSettings);
            builder.RegisterInstance(coreSettings).AsSelf();

            //main modules
            builder.RegisterModule<CQRSModule>();
            builder.RegisterModule(new RedisEventSourcingModule(_configuration));
            builder.RegisterModule(new RedisCacheModule(_configuration));
            builder.RegisterModule(new AzureBlobStorageModule(_configuration));
            builder.RegisterModule(new EthereumModule(_configuration));
            //integrations
            builder.RegisterModule<RampModule>();
            builder.RegisterModule(new TransakModule(_configuration));
            builder.RegisterModule(new BinanceModule(_configuration));
            //register automapper
            var config = new MapperConfiguration(cfg =>
            {
                if (_mappingProfileTypes != null && _mappingProfileTypes.Count > 0)
                    cfg.AddMaps(_mappingProfileTypes);
            });

            var mapper = config.CreateMapper();
            builder.RegisterInstance(mapper).As<IMapper>();
            
            switch (coreSettings.EventsModule)
            {
                case Constants.EventsModules.RabbitMQ:
                    builder.RegisterModule(new RabbitMQEventModule(_configuration));
                    builder.RegisterType<DurableRabbitMqEventPublisher>().As<IEventPublisher>();
                    break;
                case Constants.EventsModules.ServiceBus:
                    builder.RegisterModule(new ServiceBusEventModule(_configuration));
                    builder.RegisterType<ServiceBusPublisher>().As<IEventPublisher>();
                    break;
                default:
                    throw new Exception("Invalid EventsModule in configuration");
            }

            // others
            builder.RegisterType<AutofacUrlResolver>().As<IUrlResolver>();
            builder.RegisterType<DefaultStringHasher>().As<IStringHasher>();
            builder.RegisterType<DateTimeProvider>().As<IDateTimeProvider>();
            builder.RegisterType<RandomStringGenerator>().As<IStringGenerator>();
        }
    }
}