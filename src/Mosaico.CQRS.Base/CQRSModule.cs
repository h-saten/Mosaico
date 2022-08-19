using Autofac;
using MediatR;
using Mosaico.CQRS.Base.Pipelines;

namespace Mosaico.CQRS.Base
{
    public class CQRSModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterGeneric(typeof(ValidationBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(LoggingBehaviour<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(PerformanceCheckBehavior<,>)).As(typeof(IPipelineBehavior<,>));
        }
    }
}