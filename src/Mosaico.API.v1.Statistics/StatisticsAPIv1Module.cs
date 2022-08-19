using System;
using Autofac;
using Mosaico.Application.Statistics;

namespace Mosaico.API.v1.Statistics
{
    public class StatisticsAPIv1Module : Module
    {
        public static readonly Type MappingProfileType = StatisticsApplicationModule.MappingProfileType;

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterModule<StatisticsApplicationModule>();
        }
    }
}