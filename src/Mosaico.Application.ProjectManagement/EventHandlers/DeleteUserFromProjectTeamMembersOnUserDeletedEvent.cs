using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Events.Base;
using Mosaico.Events.Identity;
using Serilog;

namespace Mosaico.Application.ProjectManagement.EventHandlers
{
    [EventInfo(nameof(DeleteUserFromProjectTeamMembersOnUserDeletedEvent), "projects:api")]
    [EventTypeFilter(typeof(UserDeletedEvent))]
    public class DeleteUserFromProjectTeamMembersOnUserDeletedEvent : EventHandlerBase
    {
        private readonly IProjectDbContext _context;
        private readonly ILogger _logger;

        public DeleteUserFromProjectTeamMembersOnUserDeletedEvent(IProjectDbContext context, ILogger logger = null)
        {
            _logger = logger;
            _context = context;
        }

        public override async Task HandleAsync(CloudEvent @event)
        {
            var userEvent = @event?.GetData<UserDeletedEvent>();
            if (userEvent != null)
            {
                using (var transaction = _context.BeginTransaction())
                {
                    try
                    {
                        var projectTeamMembers =
                            await _context.ProjectMembers.Where(x => x.UserId == userEvent.Id).ToListAsync();
                        foreach (var ptm in projectTeamMembers) _context.ProjectMembers.Remove(ptm);

                        await _context.SaveChangesAsync();

                        await transaction.CommitAsync();
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
        }
    }
}