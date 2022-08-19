using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.Exceptions.Affiliation;
using Mosaico.Authorization.Base;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Events.Base;
using Mosaico.Events.ProjectManagement;

namespace Mosaico.Application.ProjectManagement.Commands.Affiliation.RewardPartner
{
    // public class RewardPartnerCommandHandler : IRequestHandler<RewardPartnerCommand>
    // {
    //     private readonly IProjectDbContext _projectDbContext;
    //     private readonly IEventPublisher _eventPublisher;
    //     private readonly IEventFactory _eventFactory;
    //     private readonly ICurrentUserContext _userContext;
    //
    //     public RewardPartnerCommandHandler(IProjectDbContext projectDbContext, IEventPublisher eventPublisher, IEventFactory eventFactory, ICurrentUserContext userContext)
    //     {
    //         _projectDbContext = projectDbContext;
    //         _eventPublisher = eventPublisher;
    //         _eventFactory = eventFactory;
    //         _userContext = userContext;
    //     }
    //
    //     public async Task<Unit> Handle(RewardPartnerCommand request, CancellationToken cancellationToken)
    //     {
    //         var partner = await _projectDbContext.Partners.Include(p => p.PartnerTransactions).FirstOrDefaultAsync(
    //             p => p.Id == request.PartnerId && p.ProjectId == request.ProjectId, cancellationToken);
    //         
    //         if (partner == null)
    //         {
    //             throw new PartnerNotFoundException(request.PartnerId);
    //         }
    //
    //         var transactions = partner.PartnerTransactions.Where(t => !t.RewardClaimed);
    //         if (transactions.Any())
    //         {
    //             var e = _eventFactory.CreateEvent(Events.ProjectManagement.Constants.EventPaths.Projects, new RewardPartnerEvent(partner.Id,  request.ProjectId, _userContext.UserId));
    //             await _eventPublisher.PublishAsync(e);
    //         }
    //         
    //         return Unit.Value;
    //     }
    // }
}