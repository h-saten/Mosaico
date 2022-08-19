using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.CounterProviders;
using Mosaico.Application.ProjectManagement.Exceptions;
using Mosaico.Application.ProjectManagement.Permissions;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Events.Base;
using Mosaico.Events.ProjectManagement;
using Mosaico.Integration.SignalR.Abstractions;
using Serilog;

namespace Mosaico.Application.ProjectManagement.EventHandlers
{
    [EventInfo(nameof(UpdateUserPermissionOnMembershipUpdated),  "projects:api")]
    [EventTypeFilter(typeof(ProjectMemberUpdatedEvent))]
    public class UpdateUserPermissionOnMembershipUpdated : EventHandlerBase
    {
        private readonly IProjectDbContext _projectDb;
        private readonly ILogger _logger;
        private readonly IProjectPermissionFactory _permissionFactory;
        private readonly ProjectCounterProvider _projectCounterProvider;
        private readonly ICountersDispatcher _countersDispatcher;

        public UpdateUserPermissionOnMembershipUpdated(IProjectDbContext projectDb, IProjectPermissionFactory permissionFactory, ProjectCounterProvider projectCounterProvider, ICountersDispatcher countersDispatcher, ILogger logger = null)
        {
            _projectDb = projectDb;
            _permissionFactory = permissionFactory;
            _projectCounterProvider = projectCounterProvider;
            _countersDispatcher = countersDispatcher;
            _logger = logger;
        }

        public override async Task HandleAsync(CloudEvent @event)
        {
            var projectEvent = @event.GetData<ProjectMemberUpdatedEvent>();
            if (projectEvent != null)
            {
                var membership = await _projectDb.ProjectMembers
                    .Include(p => p.Role)
                    .Include(p => p.Project)
                    .FirstOrDefaultAsync(p => p.ProjectId == projectEvent.ProjectId && p.Id == projectEvent.MemberId);
                
                if (membership == null)
                {
                    throw new ProjectMemberNotFoundException(projectEvent.MemberId);
                }

                if (!string.IsNullOrEmpty(membership.UserId))
                {
                    _logger?.Verbose($"User accepted invitation. Removing existing permissions");
                    await _permissionFactory.RemovePermissionsAsync(membership.ProjectId, membership.UserId);
                    await _permissionFactory.RemovePermissionsAsync(membership.Project.PageId, membership.UserId);
                    _logger?.Verbose($"Assigning new permissions to the user");
                    var rolePermissions = await _permissionFactory.GetRolePermissionsAsync(membership.Role.Key);
                    await _permissionFactory.AddUserPermissionsAsync(membership.ProjectId, membership.UserId, rolePermissions);
                    if (membership.Project.PageId.HasValue)
                    {
                        await _permissionFactory.AddUserPermissionsAsync(membership.Project.PageId.Value,
                            membership.UserId, rolePermissions);
                    }
                    
                    var counters = await _projectCounterProvider.GetCountersAsync(membership.UserId);
                    await _countersDispatcher.DispatchCounterAsync(membership.UserId, counters);
                }
                else
                {
                    _logger?.Verbose($"User didn't accept invite yet. No permissions to update");
                }
            }
        }
    }
}