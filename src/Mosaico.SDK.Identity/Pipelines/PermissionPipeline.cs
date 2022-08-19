using System;
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
    public class PermissionPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IUserManagementClient _managementClient;
        private readonly ICurrentUserContext _currentUserContext;

        public PermissionPipeline(IUserManagementClient managementClient, ICurrentUserContext currentUserContext)
        {
            _managementClient = managementClient;
            _currentUserContext = currentUserContext;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var attributes = request.GetType().GetCustomAttributes<RestrictedAttribute>();
            if (attributes.Any() && !_currentUserContext.IsGlobalAdmin)
            {
                if (!_currentUserContext.IsAuthenticated)
                {
                    throw new ForbiddenException();
                }
                foreach (var attribute in attributes)
                {
                    if (!string.IsNullOrWhiteSpace(attribute.Permission) && !string.IsNullOrWhiteSpace(attribute.RestrictedPropertyName))
                    {
                        await EvaluateRestrictedPropertyAsync(request, cancellationToken, attribute);
                    }
                    else if (attribute.Permissions.Any())
                    {
                        await EvaluateGlobalUserPermissionsAsync(cancellationToken, attribute);
                    }
                }
            }
            return await next();
        }

        private async Task EvaluateGlobalUserPermissionsAsync(CancellationToken cancellationToken,
            RestrictedAttribute attribute)
        {
            if (attribute.Permissions.Contains(Authorization.Base.Constants.DefaultRoles.Admin) &&
                !_currentUserContext.IsGlobalAdmin)
            {
                throw new ForbiddenException();
            }

            var userPermission =
                await _managementClient.GetUserPermissionsAsync(_currentUserContext.UserId.ToString(), cancellationToken);
            foreach (var permission in attribute.Permissions)
            {
                if (!userPermission.Any(up => up.Key == permission && up.EntityId == null))
                {
                    throw new ForbiddenException();
                }
            }
        }

        private async Task EvaluateRestrictedPropertyAsync(TRequest request, CancellationToken cancellationToken,
            RestrictedAttribute attribute)
        {
            var property = request.GetType().GetProperty(attribute.RestrictedPropertyName);
            if (property != null)
            {
                var propertyValue = property.GetValue(request)?.ToString();
                if (!string.IsNullOrWhiteSpace(propertyValue))
                {
                    if (attribute.Permission == Authorization.Base.Constants.DefaultRoles.Self)
                    {
                        if (!string.Equals(_currentUserContext.UserId, propertyValue, StringComparison.InvariantCultureIgnoreCase))
                        {
                            throw new ForbiddenException();
                        }
                    }
                    else if (attribute.Permission == Authorization.Base.Constants.DefaultRoles.Admin &&
                             !_currentUserContext.IsGlobalAdmin)
                    {
                        throw new ForbiddenException();
                    }
                    else if(Guid.TryParse(propertyValue, out var id))
                    {
                        await EvaluateEntityPermissionsAsync(cancellationToken, attribute, id);
                    }
                    else
                    {
                        throw new ForbiddenException();
                    }
                }
            }
        }

        private async Task EvaluateEntityPermissionsAsync(CancellationToken cancellationToken, RestrictedAttribute attribute, Guid id)
        {
            var permissions = await _managementClient.GetUserPermissionsAsync(_currentUserContext.UserId,
                id, cancellationToken);
            if (!permissions.Any(p => p.Key == attribute.Permission && p.EntityId == id))
            {
                throw new ForbiddenException();
            }
        }
    }
}