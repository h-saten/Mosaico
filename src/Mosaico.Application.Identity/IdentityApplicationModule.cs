using System;
using Autofac;
using MediatR;
using MediatR.Extensions.Autofac.DependencyInjection;
using Mosaico.Application.Identity.Abstractions;
using Mosaico.Application.Identity.Services;
using Mosaico.Application.Identity.BackgroundJobs;
using Mosaico.Application.Identity.Pipelines;
using Mosaico.BackgroundJobs.Hangfire.Extensions;
using Mosaico.Domain.Identity;
using Mosaico.SDK.BusinessManagement;
using Mosaico.SDK.ProjectManagement;
using Mosaico.Validation.Base.Extensions;

namespace Mosaico.Application.Identity
{
    public class IdentityApplicationModule : Module
    {
        public static readonly Type MappingProfileType = typeof(IdentityMapperProfile);

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterMediatR(ThisAssembly);
            //modules
            builder.RegisterModule<IdentityModule>();
            builder.RegisterModule<BusinessManagementSDKModule>();
            builder.RegisterModule<ProjectManagementSDKModule>();
            //validators
            builder.RegisterAssemblyValidators(ThisAssembly);
            //services
            builder.RegisterType<IdentityEmailService>().As<IIdentityEmailService>();
            builder.RegisterType<SecurityCodeGenerator>().As<ISecurityCodeGenerator>();
            builder.RegisterType<UserLoginAttemptFactory>().As<IUserLoginAttemptFactory>();
            builder.RegisterType<DeviceAuthorizationVerifier>().As<IDeviceAuthorizationVerifier>();
            builder.RegisterType<KycService>().As<IKycService>();
            //jobs
            builder.RegisterHangfireJob<UserAccountDeletionJob>();
            builder.RegisterHangfireJob<UserKycVerificationJob>();
            builder.RegisterHangfireJob<UserCounterJob>();
            builder.RegisterGeneric(typeof(PermissionPipeline<,>)).As(typeof(IPipelineBehavior<,>));
            //others
        }
    }
}