using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mosaico.Authorization.Base;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Events.Base;
using Mosaico.Events.Identity;
using Mosaico.SDK.Identity.Abstractions;
using Mosaico.SDK.Identity.Extensions;

namespace Mosaico.Application.Wallet.Permissions
{
    public class TokenPermissionFactory : ITokenPermissionFactory
    {
        private readonly IUserManagementClient _managementClient;
        private readonly IEventPublisher _eventPublisher;
        private readonly ICurrentUserContext _currentUserContext;
        private readonly IEventFactory _eventFactory;

        public TokenPermissionFactory(IUserManagementClient managementClient, IEventPublisher eventPublisher, ICurrentUserContext currentUserContext, IEventFactory eventFactory)
        {
            _managementClient = managementClient;
            _eventPublisher = eventPublisher;
            _currentUserContext = currentUserContext;
            _eventFactory = eventFactory;
        }
        
        public virtual async Task<TokenPermissions> GetTokenPermissionsAsync(Token token, string userId = null, CancellationToken t = new())
        {
            var permissions = new TokenPermissions
            {
                [Authorization.Base.Constants.Permissions.Token.CanRead] = false,
                [Authorization.Base.Constants.Permissions.Token.CanEdit] = false
            };

            if (!string.IsNullOrWhiteSpace(userId))
            {
                var userPermissions = await _managementClient.GetUserPermissionsAsync(userId, token.Id, t);
                permissions[Authorization.Base.Constants.Permissions.Token.CanEdit] =
                    userPermissions.HasPermission(Authorization.Base.Constants.Permissions.Token.CanEdit, token.Id) ||
                    _currentUserContext.IsGlobalAdmin;
                permissions[Authorization.Base.Constants.Permissions.Token.CanRead] =
                    permissions[Authorization.Base.Constants.Permissions.Token.CanEdit];
            }

            return permissions;
        }
        
        public virtual async Task AddUserPermissionsAsync(Guid tokenId, string userId, List<string> permissions)
        {
            var newPermissions = permissions.ToDictionary<string, string, Guid?>(permission => permission, permission => tokenId);
            if (newPermissions.Any())
            {
                var payload = new AddUserPermissionsRequested(userId, newPermissions);
                var @event = _eventFactory.CreateEvent(Events.Identity.Constants.EventPaths.Users, payload);
                await _eventPublisher.PublishAsync(@event);
            }
        }

        public virtual Task<List<string>> GetRolePermissionsAsync()
        {
            return Task.FromResult(Authorization.Base.Constants.Permissions.Token.GetAll());
        }
        
    }
}