using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Mosaico.SDK.Features.Abstractions;
using Mosaico.SDK.Features.Attributes;
using System.Reflection;
using Mosaico.Authorization.Base;
using Mosaico.Base.Exceptions;

namespace Mosaico.SDK.Features.Pipelines
{
    public class FeaturePipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IFeatureGuard _featureGuard;
        private readonly ICurrentUserContext _currentUserContext;

        public FeaturePipeline(IFeatureGuard featureGuard, ICurrentUserContext currentUserContext)
        {
            _featureGuard = featureGuard;
            _currentUserContext = currentUserContext;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var attributes = request.GetType().GetCustomAttributes<FeatureAttribute>();
            if (attributes.Any() && !_currentUserContext.IsGlobalAdmin)
            {
                foreach (var attribute in attributes.Where(a => !string.IsNullOrWhiteSpace(a.FeatureName)))
                {
                    var canExecute = await _featureGuard.CanExecuteAsync(attribute.FeatureName, _currentUserContext.UserId.ToString(), cancellationToken);
                    if (!canExecute)
                    {
                        throw new ForbiddenException();
                    }
                }
                
            }
            return await next();
        }
    }
}