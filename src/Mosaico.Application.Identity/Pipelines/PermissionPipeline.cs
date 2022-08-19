using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Mosaico.Authorization.Base;
using Mosaico.Base.Exceptions;
using Mosaico.Domain.Identity.Abstractions;

namespace Mosaico.Application.Identity.Pipelines
{
    public class PermissionPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ICurrentUserContext _currentUserContext;
        private readonly IUserReadRepository _readRepository;

        public PermissionPipeline(ICurrentUserContext currentUserContext, IUserReadRepository readRepository)
        {
            _currentUserContext = currentUserContext;
            _readRepository = readRepository;
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
                    if (!string.IsNullOrWhiteSpace(attribute.Permission) &&
                        !string.IsNullOrWhiteSpace(attribute.RestrictedPropertyName))
                    {
                        var property = request.GetType().GetProperty(attribute.RestrictedPropertyName);
                        if (property != null)
                        {
                            var propertyValue = property.GetValue(request)?.ToString();
                            if (!string.IsNullOrWhiteSpace(propertyValue))
                            {
                                if (attribute.Permission == Authorization.Base.Constants.DefaultRoles.Self)
                                {
                                    if (_currentUserContext.UserId.ToUpperInvariant() != propertyValue.ToUpperInvariant())
                                    {
                                        throw new ForbiddenException();
                                    }
                                }
                                else if(Guid.TryParse(propertyValue, out var id))
                                {
                                    var permissions = await _readRepository.GetUserPermissionsAsync(_currentUserContext.UserId.ToString(), id, cancellationToken);
                                    if (!permissions.Any(p => p.Permission.Key == attribute.Permission && p.EntityId == id))
                                    {
                                        throw new ForbiddenException();
                                    }
                                }
                                else
                                {
                                    throw new ForbiddenException();
                                }
                            }
                        }
                    }
                    else if (attribute.Permissions.Any())
                    {
                        var userPermission = await _readRepository.GetUserPermissionsAsync(_currentUserContext.UserId.ToString(), cancellationToken);
                        foreach (var permission in attribute.Permissions)
                        {
                            if (!userPermission.Any(up => up.Permission.Key == permission && up.EntityId == null))
                            {
                                throw new ForbiddenException();
                            }
                        }
                    }
                }
            }
            return await next();
        }
    }
}