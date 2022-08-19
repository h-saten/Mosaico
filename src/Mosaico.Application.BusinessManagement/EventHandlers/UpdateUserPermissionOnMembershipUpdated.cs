using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.BusinessManagement.CounterProviders;
using Mosaico.Domain.BusinessManagement.Exceptions;
using Mosaico.Application.BusinessManagement.Permissions;
using Mosaico.Domain.BusinessManagement.Abstractions;
using Mosaico.Events.Base;
using Mosaico.Events.BusinessManagement;
using Mosaico.Integration.SignalR.Abstractions;
using Serilog;

namespace Mosaico.Application.BusinessManagement.EventHandlers
{
    [EventInfo(nameof(UpdateUserPermissionOnMembershipUpdated),  "companies:api")]
    [EventTypeFilter(typeof(CompanyInvitationUpdatedEvent))]
    public class UpdateUserPermissionOnMembershipUpdated : EventHandlerBase
    {
        private readonly IBusinessDbContext _businessDb;
        private readonly ILogger _logger;
        private readonly ICompanyPermissionFactory _permissionFactory;
        private readonly CompanyCounterProvider _companyCounterProvider;
        private readonly ICountersDispatcher _countersDispatcher;

        public UpdateUserPermissionOnMembershipUpdated(IBusinessDbContext businessDb, ICompanyPermissionFactory permissionFactory, CompanyCounterProvider companyCounterProvider, ICountersDispatcher countersDispatcher, ILogger logger = null)
        {
            _businessDb = businessDb;
            _permissionFactory = permissionFactory;
            _companyCounterProvider = companyCounterProvider;
            _countersDispatcher = countersDispatcher;
            _logger = logger;
        }

        public override async Task HandleAsync(CloudEvent @event)
        {
            var companyEvent = @event.GetData<CompanyInvitationUpdatedEvent>();
            if (companyEvent != null)
            {
                var membership = await _businessDb.TeamMembers
                    .Include(p => p.TeamMemberRole)
                    .FirstOrDefaultAsync(p => p.CompanyId == companyEvent.CompanyId && p.Id == companyEvent.InvitationId);
                
                if (membership != null)
                {
                    if (!string.IsNullOrEmpty(membership.UserId))
                    {
                        _logger?.Verbose($"User accepted invitation. Removing existing permissions");
                        await _permissionFactory.RemovePermissionsAsync(membership.CompanyId, membership.UserId);
                        _logger?.Verbose($"Assigning new permissions to the user");
                        var rolePermissions = await _permissionFactory.GetRolePermissionsAsync(membership.TeamMemberRole.Key);
                        await _permissionFactory.AddUserPermissionsAsync(membership.CompanyId, membership.UserId, rolePermissions);
                        var counters = await _companyCounterProvider.GetCountersAsync(membership.UserId);
                        await _countersDispatcher.DispatchCounterAsync(membership.UserId, counters);
                    }
                    else
                    {
                        _logger?.Verbose($"User didn't accept invite yet. No permissions to update");
                    }
                }
                else
                {
                    _logger?.Warning($"Membership {companyEvent.InvitationId} not found");
                }
            }
        }
    }
}