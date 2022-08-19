using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mosaico.Authorization.Base;
using Mosaico.Domain.BusinessManagement.Entities;
using Mosaico.Domain.BusinessManagement.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.Identity;
using Mosaico.SDK.Identity.Abstractions;
using Mosaico.SDK.Identity.Extensions;
using Constants = Mosaico.Domain.BusinessManagement.Constants;

namespace Mosaico.Application.BusinessManagement.Permissions
{
    public class CompanyPermissionFactory : ICompanyPermissionFactory
    {
        private readonly IUserManagementClient _managementClient;
        private readonly IEventPublisher _eventPublisher;
        private readonly ICurrentUserContext _currentUserContext;
        private readonly IEventFactory _eventFactory;
        
        public CompanyPermissionFactory(IUserManagementClient managementClient, IEventPublisher eventPublisher, IEventFactory eventFactory, ICurrentUserContext currentUserContext)
        {
            _managementClient = managementClient;
            _eventPublisher = eventPublisher;
            _eventFactory = eventFactory;
            _currentUserContext = currentUserContext;
        }

        public virtual async Task<CompanyPermissions> CreateCompanyPermissionsAsync(Company company, string userId = null, CancellationToken t = new())
        {
            var permissions = new CompanyPermissions
            {
                [Authorization.Base.Constants.Permissions.Company.CanRead] = true
            };

            if (!string.IsNullOrWhiteSpace(userId))
            {
                var userPermissions = await _managementClient.GetUserPermissionsAsync(userId, t);
                permissions[Authorization.Base.Constants.Permissions.Company.CanRead] = true;
                permissions[Authorization.Base.Constants.Permissions.Company.CanEditDetails] =
                    userPermissions.HasPermission(Authorization.Base.Constants.Permissions.Company.CanEditDetails, company.Id) || _currentUserContext.IsGlobalAdmin;
                permissions[Authorization.Base.Constants.Permissions.Company.CanReadCompanyWallet] =  userPermissions.HasPermission(Authorization.Base.Constants.Permissions.Company.CanReadCompanyWallet, company.Id);
            }

            return permissions;
        }
        
        public virtual async Task AddUserPermissionsAsync(Guid companyId, string userId, List<string> permissions)
        {
            var newPermissions = permissions.ToDictionary<string, string, Guid?>(permission => permission, permission => companyId);
            if (newPermissions.Any())
            {
                var payload = new AddUserPermissionsRequested(userId, newPermissions);
                var @event = _eventFactory.CreateEvent(Events.Identity.Constants.EventPaths.Users, payload);
                await _eventPublisher.PublishAsync(@event);
            }
        }

        public virtual async Task RemovePermissionsAsync(Guid? projectId, string userId, List<string> permissions = null, CancellationToken t = new())
        {
            permissions ??= Authorization.Base.Constants.Permissions.Company.GetAll();
            var permissionsToDelete = permissions.ToDictionary(s => s, s => projectId);
            if (permissionsToDelete.Any())
            {
                var payload = new RemoveUserPermissionsRequested(userId, permissionsToDelete);
                var @event = _eventFactory.CreateEvent(Events.Identity.Constants.EventPaths.Users, payload);
                await _eventPublisher.PublishAsync(@event);
            }
        }

        public virtual Task<List<string>> GetRolePermissionsAsync(string role)
        {
            switch (role)
            {
                case Domain.BusinessManagement.Constants.TeamMemberRoles.Owner:
                    return Task.FromResult(Authorization.Base.Constants.Permissions.Company.GetAll());
                case Domain.BusinessManagement.Constants.TeamMemberRoles.Member:
                    return Task.FromResult(new List<string>
                    {
                        Authorization.Base.Constants.Permissions.Company.CanRead,
                        Authorization.Base.Constants.Permissions.Company.CanReadCompanyWallet
                    });
                default:
                    throw new CompanyRoleNotFoundException(role);
            }
        }

    }
}