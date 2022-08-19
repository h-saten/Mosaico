using System.Threading.Tasks;
using Mosaico.Application.BusinessManagement.Permissions;
using Mosaico.Events.Base;
using Mosaico.Events.BusinessManagement;
using Serilog;

namespace Mosaico.Application.BusinessManagement.EventHandlers
{
    [EventInfo(nameof(DeleteUserPermissionsOnMemberDeleted),  "companies:api")]
    [EventTypeFilter(typeof(CompanyInvitationDeletedEvent))]
    public class DeleteUserPermissionsOnMemberDeleted : EventHandlerBase
    {
        private readonly ILogger _logger;
        private readonly ICompanyPermissionFactory _permissionFactory;
        
        public DeleteUserPermissionsOnMemberDeleted(ICompanyPermissionFactory permissionFactory, ILogger logger = null)
        {
            _permissionFactory = permissionFactory;
            _logger = logger;
        }

        public override async Task HandleAsync(CloudEvent @event)
        {
            var companyEvent = @event.GetData<CompanyInvitationDeletedEvent>();
            if (companyEvent != null)
            {
                if (!string.IsNullOrEmpty(companyEvent.UserId))
                {
                    _logger?.Verbose($"User ID is not empty. Invitation was accepted and permissions should be cleaned up");
                    await _permissionFactory.RemovePermissionsAsync(companyEvent.CompanyId, companyEvent.UserId);
                    _logger?.Verbose($"All permissions were successfully deleted");
                }
                else
                {
                    _logger?.Verbose($"User didn't accept invitation. Nothing to clean");
                }
            }
        }
    }
}