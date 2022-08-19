using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Mosaico.Events.Base;
using Mosaico.Events.Base.Extensions;
using Mosaico.Events.ServiceBus.Configurations;
using System.Reflection;
using Module = Autofac.Module;

namespace Mosaico.Events.ServiceBus
{
    public class ServiceBusEventModule : Module
    {
        private readonly IConfiguration _configuration;

        public ServiceBusEventModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            var config = new ServiceBusConfiguration();
            _configuration.GetSection(ServiceBusConfiguration.SectionName).Bind(config);
            builder.RegisterInstance(config);
            builder.RegisterType<ServiceBusPublisher>().As<IEventPublisher>();
            
            builder.AddMassTransit(x =>
            {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                var eventHandlerTypes = assemblies.SelectMany(x => x.GetTypesWithEventAttribute());
                var endpointMapping = new Dictionary<string, List<string>>();
                foreach (var handler in eventHandlerTypes)
                {
                    if (handler.GetCustomAttribute<EventInfoAttribute>() is EventInfoAttribute attribute)
                    {
                        var subDetails = attribute.GetSubscriptionDetails();
                        if (subDetails.HasValue)
                        {
                            if (!endpointMapping.ContainsKey(subDetails.Value.Subscription))
                            {
                                endpointMapping[subDetails.Value.Subscription] = new List<string>();
                            }
                            if(!endpointMapping[subDetails.Value.Subscription].Contains(subDetails.Value.Topic))
                            {
                                endpointMapping[subDetails.Value.Subscription].Add(subDetails.Value.Topic);
                            }
                        }
                    }
                }
            
                var handlerType = typeof(ServiceBusMessageAggregator);
                x.AddConsumer(handlerType);
                
                x.UsingAzureServiceBus((context, cfg) =>
                {
                    cfg.Host(config.ConnectionString);
                    foreach (var queue in endpointMapping)
                    {
                        foreach (var topic in queue.Value)
                        {
                            cfg.SubscriptionEndpoint($"{queue.Key}_{config.Prefix}", topic, configurator =>
                            {
                                configurator.ConfigureConsumer(context, handlerType);
                            });
                        }
                    }
                    cfg.ConfigureEndpoints(context);
                    
                });
            });
            builder.RegisterType<CloudEventFactory>().As<IEventFactory>();
        }
    }
}