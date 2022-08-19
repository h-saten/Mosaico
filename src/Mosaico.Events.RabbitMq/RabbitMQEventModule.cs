using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using MassTransit;
using Mosaico.Events.Base;
using Mosaico.Events.Base.Extensions;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Mosaico.Events.RabbitMq.Configurations;
using Module = Autofac.Module;

namespace Mosaico.Events.RabbitMq
{
    public class RabbitMQEventModule : Module
    {
        private readonly IConfiguration _configuration;

        public RabbitMQEventModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            var rabbitMqConfig = new RabbitMQConfiguration();
            _configuration.GetSection(RabbitMQConfiguration.SectionName).Bind(rabbitMqConfig);
            builder.RegisterInstance(rabbitMqConfig);
            
            builder.RegisterType<RabbitMQPublisher>().As<IEventPublisher>();
            
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

                var handlerType = typeof(RabbitMQMessageAggregator);
                x.AddConsumer(handlerType);
                
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(new Uri(rabbitMqConfig.Host));
                    foreach (var queue in endpointMapping)
                    {
                        cfg.ReceiveEndpoint($"{queue.Key}_{rabbitMqConfig.Prefix}", e =>
                        {
                            e.Durable = true;
                            foreach (var endpoint in endpointMapping[queue.Key])
                            {
                                e.Bind(endpoint);
                            }
                        
                            e.ConfigureConsumer(context, handlerType);
                        });
                    }
                    
                });
            });
            builder.RegisterType<CloudEventFactory>().As<IEventFactory>();
        }
    }
}