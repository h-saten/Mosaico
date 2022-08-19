using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.BusinessManagement.CounterProviders;
using Mosaico.Domain.BusinessManagement.Abstractions;
using Mosaico.Events.Base;
using Mosaico.Events.Identity;
using Mosaico.Integration.Email.Abstraction;
using Mosaico.Integration.SignalR.Abstractions;
using Serilog;

namespace Mosaico.Application.BusinessManagement.EventHandlers
{
    [EventInfo(nameof(DeleteUserMembershipOnUserDeletedEvent), "companies:api")]
    [EventTypeFilter(typeof(UserDeletedEvent))]
    public class DeleteUserMembershipOnUserDeletedEvent : EventHandlerBase
    {
        private readonly ILogger _logger;
        private readonly IBusinessDbContext _context;
        private readonly CompanyCounterProvider _companyCounterProvider;
        private readonly ICountersDispatcher _countersDispatcher;

        public DeleteUserMembershipOnUserDeletedEvent(IBusinessDbContext context, CompanyCounterProvider companyCounterProvider, ICountersDispatcher countersDispatcher, ILogger logger = null)
        {
            _logger = logger;
            _context = context;
            _companyCounterProvider = companyCounterProvider;
            _countersDispatcher = countersDispatcher;
        }

        public override async Task HandleAsync(CloudEvent @event)
        {
            var userEvent = @event?.GetData<UserDeletedEvent>();
            if (userEvent != null)
            {
                var userId = userEvent.Id;

                using (var transaction = _context.BeginTransaction())
                {
                    try
                    {
                        var companyInvitations = await _context.TeamMembers.Where(x => x.UserId == userId).ToListAsync();
                        foreach (var inv in companyInvitations)
                        {
                            _context.TeamMembers.Remove(inv);
                        }
                        var companyTeamMembers = await _context.TeamMembers.Where(x => x.UserId == userId).ToListAsync();
                        foreach (var ctm in companyTeamMembers)
                        {
                            _context.TeamMembers.Remove(ctm);
                        }
                        await _context.SaveChangesAsync();

                        await transaction.CommitAsync();
                        
                        var counters = await _companyCounterProvider.GetCountersAsync(userId);
                        await _countersDispatcher.DispatchCounterAsync(userId, counters);
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