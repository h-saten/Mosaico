using System;
using Autofac;
using Microsoft.Extensions.Configuration;
using Mosaico.Application.Identity;
using Mosaico.Authorization.Base.Configurations;
using Mosaico.BackgroundJobs.Hangfire;

namespace Mosaico.API.v1.Identity
{
    public class IdentityAPIv1Module : Module
    {
        public static readonly Type MappingProfileType = IdentityApplicationModule.MappingProfileType;
        
        private readonly IConfiguration _configuration;

        public IdentityAPIv1Module(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterModule<IdentityApplicationModule>();
            builder.RegisterModule(new IdentityEventModule(_configuration));
        }
    }
}