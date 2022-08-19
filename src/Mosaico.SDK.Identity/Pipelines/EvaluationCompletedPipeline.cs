using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Mosaico.Authorization.Base;
using Mosaico.Base.Exceptions;
using Mosaico.SDK.Identity.Abstractions;

namespace Mosaico.SDK.Identity.Pipelines
{
    public class EvaluationCompletedPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IUserManagementClient _managementClient;
        private readonly ICurrentUserContext _currentUserContext;

        public EvaluationCompletedPipeline(IUserManagementClient managementClient, ICurrentUserContext currentUserContext)
        {
            _managementClient = managementClient;
            _currentUserContext = currentUserContext;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var attributes = request.GetType().GetCustomAttributes<ShouldCompleteEvaluationAttribute>();
            if (attributes.Any())
            {
                if (!_currentUserContext.IsAuthenticated)
                {
                    throw new ForbiddenException();
                }

                var user = await _managementClient.GetUserAsync(_currentUserContext.UserId, cancellationToken);
                if (user == null || !user.EvaluationCompleted)
                {
                    throw new ForbiddenException();
                }
            }
            return await next();
        }
    }
}