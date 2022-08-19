using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.Exceptions.Airdrop;
using Mosaico.Authorization.Base;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Events.Base;
using Mosaico.Events.ProjectManagement;

namespace Mosaico.Application.ProjectManagement.Commands.Airdrop.DistributeAirdrop
{
    public class DistributeAirdropCommandHandler : IRequestHandler<DistributeAirdropCommand>
    {
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly IProjectDbContext _projectDbContext;
        private readonly ICurrentUserContext _userContext;

        public DistributeAirdropCommandHandler(IProjectDbContext projectDbContext, IEventFactory eventFactory,
            IEventPublisher eventPublisher, ICurrentUserContext userContext)
        {
            _projectDbContext = projectDbContext;
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _userContext = userContext;
        }

        public async Task<Unit> Handle(DistributeAirdropCommand request, CancellationToken cancellationToken)
        {
            var airDrop = await _projectDbContext.AirdropCampaigns.Include(a => a.Participants).Include(a => a.Project)
                .FirstOrDefaultAsync(
                    a => a.Id == request.AirdropId && a.ProjectId == request.ProjectId, cancellationToken);
            if (airDrop == null) throw new AirdropNotFoundException(request.AirdropId.ToString());

            var usersToReward = airDrop.Participants.Where(p => p.Claimed && !p.WithdrawnAt.HasValue)
                .Select(a => a.WalletAddress);
            if (usersToReward.Any())
            {
                var e = _eventFactory.CreateEvent(Events.ProjectManagement.Constants.EventPaths.Projects, new DistributeAirdropEvent(airDrop.Id, _userContext.UserId));
                await _eventPublisher.PublishAsync(e);
            }
            return Unit.Value;
        }
    }
}