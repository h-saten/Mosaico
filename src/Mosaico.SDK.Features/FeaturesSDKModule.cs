using Autofac;
using MediatR;
using Mosaico.SDK.Features.Abstractions;
using Mosaico.SDK.Features.Guards;
using Mosaico.SDK.Features.Pipelines;

namespace Mosaico.SDK.Features
{
    public class FeaturesSDKModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<FeaturesClient>().As<IFeaturesClient>();
            builder.RegisterType<FeatureGuard>().As<IFeatureGuard>();
            builder.RegisterGeneric(typeof(FeaturePipeline<,>)).As(typeof(IPipelineBehavior<,>));
        }
    }
}