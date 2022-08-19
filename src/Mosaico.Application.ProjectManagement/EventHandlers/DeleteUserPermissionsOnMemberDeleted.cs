using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.CounterProviders;
using Mosaico.Application.ProjectManagement.Permissions;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Events.Base;
using Mosaico.Events.ProjectManagement;
using Mosaico.Integration.SignalR.Abstractions;
using Serilog;

namespace Mosaico.Application.ProjectManagement.EventHandlers
{
    [EventInfo(nameof(DeleteUserPermissionsOnMemberDeleted),  "projects:api")]
    [EventTypeFilter(typeof(ProjectMemberDeletedEvent))]
    public class DeleteUserPermissionsOnMemberDeleted : EventHandlerBase
    {
        private readonly ILogger _logger;
        private readonly IProjectDbContext _context;
        private readonly IProjectPermissionFactory _permissionFactory;
        private readonly ProjectCounterProvider _projectCounterProvider;
        private readonly ICountersDispatcher _countersDispatcher;

        public DeleteUserPermissionsOnMemberDeleted(IProjectPermissionFactory permissionFactory, IProjectDbContext context, ProjectCounterProvider projectCounterProvider, ICountersDispatcher countersDispatcher, ILogger logger = null)
        {
            _permissionFactory = permissionFactory;
            _context = context;
            _projectCounterProvider = projectCounterProvider;
            _countersDispatcher = countersDispatcher;
            _logger = logger;
        }

        public override async Task HandleAsync(CloudEvent @event)
        {
            var projectEvent = @event.GetData<ProjectMemberDeletedEvent>();
            if (projectEvent != null)
            {
                if (!string.IsNullOrEmpty(projectEvent.UserId))
                {
                    _logger?.Verbose($"User ID is not empty. Invitation was accepted and permissions should be cleaned up");
                    await _permissionFactory.RemovePermissionsAsync(projectEvent.ProjectId, projectEvent.UserId);
                    _logger?.Verbose($"All permissions were successfully deleted");
                    var page = await _context.TokenPages.FirstOrDefaultAsync(p => p.ProjectId == projectEvent.ProjectId);
                    if (page != null)
                    {
                        _logger?.Verbose($"Attempting to delete permissions of page");
                        await _permissionFactory.RemovePermissionsAsync(page.Id, projectEvent.UserId);
                        _logger?.Verbose($"All page permissions were successfully deleted");
                    }

                    var counters = await _projectCounterProvider.GetCountersAsync(projectEvent.UserId);
                    await _countersDispatcher.DispatchCounterAsync(projectEvent.UserId, counters);
                }
                else
                {
                    _logger?.Verbose($"User didn't accept invitation. Nothing to clean");
                }
            }
        }
    }
}