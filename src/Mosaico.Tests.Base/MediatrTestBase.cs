using System.Threading.Tasks;
using Autofac;
using MediatR;
using MediatR.Extensions.Autofac.DependencyInjection;
using Mosaico.CQRS.Base;

namespace Mosaico.Tests.Base
{
    public abstract class MediatrTestBase : AutofacTestBase
    {
        protected override void RegisterDependencies(ContainerBuilder builder)
        {
            base.RegisterDependencies(builder);
            builder.RegisterMediatR(GetType().Assembly);
            builder.RegisterModule(new CQRSModule());
        }

        public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            var mediatr = Container.Resolve<IMediator>();
            return await mediatr.Send(request);
        }
    }
}