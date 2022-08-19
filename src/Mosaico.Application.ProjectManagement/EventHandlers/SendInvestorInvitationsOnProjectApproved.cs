using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.ProjectManagement;

namespace Mosaico.Application.ProjectManagement.EventHandlers
{
    // [EventInfo(nameof(DeployContractsOnProjectApproved),  "projects:api")]
    // [EventTypeFilter(typeof(ProjectAcceptedEvent))]
    // public class SendInvestorInvitationsOnProjectApproved : EventHandlerBase
    // {
    //     private readonly IProjectDbContext _projectDb;
    //     private readonly IProjectEmailSender _emailSender;
    //     private readonly InvitationCounterProvider _projectCounterProvider;
    //     private readonly ICountersDispatcher _countersDispatcher;
    //
    //     public SendInvestorInvitationsOnProjectApproved(IProjectDbContext projectDb, IProjectEmailSender emailSender)
    //     {
    //         _projectDb = projectDb;
    //         _emailSender = emailSender;
    //     }
    //
    //     public override async Task HandleAsync(CloudEvent @event)
    //     {
    //         var projectEvent = @event?.GetData<ProjectAcceptedEvent>();
    //         if (projectEvent != null)
    //         {
    //             var project = await _projectDb.Projects.FirstOrDefaultAsync(p => p.Id == projectEvent.Id);
    //             if (project == null)
    //             {
    //                 throw new ProjectNotFoundException(projectEvent.Id);
    //             }
    //             
    //             var invitationsToSend = new List<VestingFundInvitation>();
    //
    //             var funds = project.Vesting?.Funds;
    //             if (funds != null && funds.Any())
    //             {
    //                 invitationsToSend = funds.SelectMany(f => f.Invitations).Where(i => !i.IsInvitationSent).ToList();
    //             }
    //         
    //             if (invitationsToSend.Any())
    //             {
    //                 //TODO: generate link
    //                 await _emailSender.SendInvestorInvitationsAsync(project,
    //                     invitationsToSend.Select(s => s.Email).ToList());
    //                 foreach (var vestingFundInvitation in invitationsToSend)
    //                 {
    //                     vestingFundInvitation.IsInvitationSent = true;
    //                     var counters = await _projectCounterProvider.GetCountersAsync(membership.UserId);
    //                     await _countersDispatcher.DispatchCounterAsync(membership.UserId, counters);
    //                 }
    //                 await _projectDb.SaveChangesAsync();
    //             }
    //         }
    //     }
    // }
}