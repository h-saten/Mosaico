using Autofac;
using Mosaico.SDK.BusinessManagement.Abstractions;
using System;

namespace Mosaico.SDK.BusinessManagement
{
    public class BusinessManagementSDKModule : Module
    {
        public static readonly Type MappingProfileType = typeof(BusinessManagementClientMapperProfile);
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<BusinessManagementClient>().As<IBusinessManagementClient>();
        }
    }
}