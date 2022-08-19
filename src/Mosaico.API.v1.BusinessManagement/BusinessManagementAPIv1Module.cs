using System;
using Autofac;
using Mosaico.Application.BusinessManagement;

namespace Mosaico.API.v1.BusinessManagement
{
    public class BusinessManagementAPIv1Module: Module
    {
        public static readonly Type MappingProfileType = BusinessManagementApplicationModule.MappingProfileType;

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterModule<BusinessManagementApplicationModule>();
        }
    }
}