using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Authorization.Base;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities;
using Mosaico.SDK.Identity.Abstractions;
using Mosaico.SDK.Identity.Extensions;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.Identity;
using Mosaico.SDK.Wallet.Abstractions;

namespace Mosaico.Application.ProjectManagement.Permissions
{
    public class ProjectPermissionFactory : IProjectPermissionFactory
    {
        private readonly IUserManagementClient _managementClient;
        private readonly IWalletClient _walletClient;
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventFactory _eventFactory;
        private readonly ICurrentUserContext _currentUserContext;
        private readonly IProjectDbContext _projectDbContext;
        
        public ProjectPermissionFactory(
            IUserManagementClient managementClient, 
            IWalletClient walletClient, 
            IEventPublisher eventPublisher, 
            IEventFactory eventFactory, 
            ICurrentUserContext currentUserContext, 
            IProjectDbContext projectDbContext)
        {
            _managementClient = managementClient;
            _walletClient = walletClient;
            _eventPublisher = eventPublisher;
            _eventFactory = eventFactory;
            _currentUserContext = currentUserContext;
            _projectDbContext = projectDbContext;
        }

        public virtual async Task<ProjectPermissions> CreateProjectPermissionsAsync(Project project, string userId = null, CancellationToken t = new())
        {
            var permissions = new ProjectPermissions();

            if (!string.IsNullOrEmpty(userId))
            {
                var userPermissions = await _managementClient.GetUserPermissionsAsync(userId, project.Id, t);

                permissions[Authorization.Base.Constants.Permissions.Project.CanEditDetails] =
                    (userPermissions.HasPermission(Authorization.Base.Constants.Permissions.Project.CanEditDetails,
                        project.Id) && project.Status.Key == Domain.ProjectManagement.Constants.ProjectStatuses.New) || _currentUserContext.IsGlobalAdmin;
                permissions[Authorization.Base.Constants.Permissions.Project.CanRead] =
                    project.IsBlockedForEditing ?
                        _currentUserContext.IsGlobalAdmin :
                    (project.Status.Key == Domain.ProjectManagement.Constants.ProjectStatuses.Approved ||
                    project.Status.Key == Domain.ProjectManagement.Constants.ProjectStatuses.InProgress ||
                    project.Status.Key == Domain.ProjectManagement.Constants.ProjectStatuses.Closed && project.IsVisible) || 
                    _currentUserContext.IsGlobalAdmin || userPermissions.HasPermission(Authorization.Base.Constants.Permissions.Project.CanRead, project.Id) || project.IsAccessibleViaLink;
                permissions[Authorization.Base.Constants.Permissions.Project.CanEditDocuments] =
                    userPermissions.HasPermission(Authorization.Base.Constants.Permissions.Project.CanEditDocuments, project.Id)
                    || _currentUserContext.IsGlobalAdmin;
                permissions[Authorization.Base.Constants.Permissions.Project.CanEditStages] =
                    userPermissions.HasPermission(Authorization.Base.Constants.Permissions.Project.CanEditStages, project.Id)
                    || _currentUserContext.IsGlobalAdmin;
                permissions[Authorization.Base.Constants.Permissions.Project.CanEditVesting] =
                    userPermissions.HasPermission(Authorization.Base.Constants.Permissions.Project.CanEditVesting, project.Id)
                    || _currentUserContext.IsGlobalAdmin;
                permissions[Authorization.Base.Constants.Permissions.Project.CanEditStaking] =
                    userPermissions.HasPermission(Authorization.Base.Constants.Permissions.Project.CanEditStaking, project.Id)
                    || _currentUserContext.IsGlobalAdmin;
                permissions[Authorization.Base.Constants.Permissions.Project.CanPurchase] = await GetUserAbilityToPurchaseAsync(project, userId, t);
                permissions[Authorization.Base.Constants.Permissions.Project.CanViewDashboard] = userPermissions.HasPermission(Authorization.Base.Constants.Permissions.Project.CanEditDetails,
                    project.Id) || _currentUserContext.IsGlobalAdmin;
            }
            else
            {
                permissions[Authorization.Base.Constants.Permissions.Project.CanRead] =
                    ((project.Status.Key == Domain.ProjectManagement.Constants.ProjectStatuses.Approved ||
                    project.Status.Key == Domain.ProjectManagement.Constants.ProjectStatuses.InProgress ||
                    project.Status.Key == Domain.ProjectManagement.Constants.ProjectStatuses.Closed) && project.IsVisible && !project.IsBlockedForEditing) || project.IsAccessibleViaLink;
            }

            return permissions;
        }

        public virtual async Task AddUserPermissionsAsync(Guid projectId, string userId, List<string> permissions)
        {
            var newPermissions = permissions.ToDictionary<string, string, Guid?>(permission => permission, permission => projectId);
            if (newPermissions.Any())
            {
                var payload = new AddUserPermissionsRequested(userId, newPermissions);
                var @event = _eventFactory.CreateEvent(Events.Identity.Constants.EventPaths.Users, payload);
                await _eventPublisher.PublishAsync(@event);
            }
        }

        public virtual async Task RemovePermissionsAsync(Guid? projectId, string userId, List<string> permissions = null, CancellationToken t = new())
        {
            permissions ??= Authorization.Base.Constants.Permissions.Project.GetAll();
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
                case Domain.ProjectManagement.Constants.Roles.Owner:
                    return Task.FromResult(Authorization.Base.Constants.Permissions.Project.GetAll());
                case Domain.ProjectManagement.Constants.Roles.Member:
                    return Task.FromResult(new List<string>
                    {
                        Authorization.Base.Constants.Permissions.Project.CanRead
                    });
                default:
                    throw new ProjectRoleNotFoundException(role);
            }
        }

        public virtual Task<bool> GetUserAbilityToPurchaseAsync(Project project, string userId, CancellationToken t = new())
        {
            var canPurchase = false;
            var currentActiveStage = project.ActiveStage();
            canPurchase = currentActiveStage != null && currentActiveStage.Status.Key == Domain.ProjectManagement.Constants.StageStatuses.Active;
            if (canPurchase)
            {
                if (currentActiveStage.Type == StageType.Private || currentActiveStage.Type == StageType.PreSale)
                {
                    canPurchase = !string.IsNullOrWhiteSpace(userId) && currentActiveStage.ProjectInvestors.Any(pi =>
                        pi.UserId == _currentUserContext.UserId && pi.IsAllowed);
                }
                //TODO: fix vesting
                // if (currentActiveStage.Vesting != null && currentActiveStage.Vesting.VestingWalletId.HasValue)
                // {
                //     var vestingWalletParticipants = await _walletClient.GetVestingParticipantsAsync(currentActiveStage.Vesting.VestingWalletId.Value);
                //     canPurchase = vestingWalletParticipants.Select(v => v.ToString()).Contains(userId);
                // }
            }

            return Task.FromResult(project.Status.Key == Domain.ProjectManagement.Constants.ProjectStatuses.InProgress &&
                   canPurchase);
        }

        public virtual async Task<bool> GetUserAbilityToPurchaseAsync(Guid projectId, string userId, CancellationToken t = new())
        {
            var project = await _projectDbContext.Projects.SingleOrDefaultAsync(x => x.Id == projectId, t);
            return await GetUserAbilityToPurchaseAsync(project, userId, t);
        }
    }
}