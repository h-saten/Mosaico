using System;
using Autofac;
using MediatR.Extensions.Autofac.DependencyInjection;
using Mosaico.Application.Statistics.Abstractions;
using Mosaico.Application.Statistics.BackgroundJobs;
using Mosaico.Application.Statistics.EventHandlers;
using Mosaico.Application.Statistics.Services;
using Mosaico.BackgroundJobs.Base;
using Mosaico.Domain.Statistics;
using Mosaico.Events.Base;
using Mosaico.SDK.ProjectManagement;
using Mosaico.Validation.Base.Extensions;

namespace Mosaico.Application.Statistics
{
    public class StatisticsApplicationModule : Module
    {
        public static readonly Type MappingProfileType = typeof(StatisticsMapperProfile);
        
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterMediatR(ThisAssembly);

            builder.RegisterModule<StatisticsModule>();
            builder.RegisterModule<ProjectManagementSDKModule>();
            builder.RegisterAssemblyValidators(ThisAssembly);

            builder.RegisterType<KPIService>().As<IKPIService>();
            builder.RegisterType<SaveProjectTransactionOnTransactionFinalized>().As<IEventHandler>();
            builder.RegisterType<UpdateUserCounterKPI>().As<IEventHandler>();
            builder.RegisterType<KPIUpdateJob>().As<IBackgroundJob>();
        }
    }
}