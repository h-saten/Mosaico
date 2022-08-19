using Autofac;
using Mosaico.SDK.ProjectManagement.Abstractions;
using System;

namespace Mosaico.SDK.ProjectManagement
{
    public class ProjectManagementSDKModule : Module
    {
        public static readonly Type MappingProfileType = typeof(ProjectManagementClientMapperProfile);
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<ProjectManagementClient>().As<IProjectManagementClient>();
        }
    }
}