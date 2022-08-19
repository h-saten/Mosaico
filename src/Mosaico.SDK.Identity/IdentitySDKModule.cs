using Autofac;
using MediatR;
using Mosaico.SDK.Identity.Abstractions;
using Mosaico.SDK.Identity.Configurations;
using Mosaico.SDK.Identity.Pipelines;

namespace Mosaico.SDK.Identity
{
    public class IdentitySDKModule : Module
    {
        private readonly IdentityServerConfiguration _configuration;

        public IdentitySDKModule(IdentityServerConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterInstance(_configuration);
            builder.RegisterType<IdentityClient>().As<IUserManagementClient>();
            builder.RegisterGeneric(typeof(PermissionPipeline<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(EvaluationCompletedPipeline<,>)).As(typeof(IPipelineBehavior<,>));
        }
    }
}