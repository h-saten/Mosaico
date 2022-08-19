using System;
using Autofac;
using Mosaico.Application.ProjectManagement;

namespace Mosaico.API.v1.ProjectManagement
{
    public class ProjectManagementAPIv1Module: Module
    {
        public static readonly Type MappingProfileType = ProjectManagementApplicationModule.MappingProfileType;

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterModule<ProjectManagementApplicationModule>();
        }
    }
}