using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.BusinessManagement.Services;
using Mosaico.Authorization.Base;
using Mosaico.Domain.BusinessManagement.Abstractions;
using Mosaico.Domain.BusinessManagement.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.BusinessManagement;

namespace Mosaico.Application.BusinessManagement.Commands.Vote
{
    public class VoteCommandHandler : IRequestHandler<VoteCommand>
    {
        private readonly IBusinessDbContext _businessDb;
        private readonly IProposalService _proposalService;
        private readonly ICurrentUserContext _currentUser;
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventFactory _eventFactory;

        public VoteCommandHandler(IBusinessDbContext businessDb, IProposalService proposalService, ICurrentUserContext currentUser, IEventPublisher eventPublisher, IEventFactory eventFactory)
        {
            _businessDb = businessDb;
            _proposalService = proposalService;
            _currentUser = currentUser;
            _eventPublisher = eventPublisher;
            _eventFactory = eventFactory;
        }

        public async Task<Unit> Handle(VoteCommand request, CancellationToken cancellationToken)
        {
            var proposal = await _businessDb.Proposals.FirstOrDefaultAsync(t =>
                t.CompanyId == request.CompanyId && t.Id == request.ProposalId, cancellationToken: cancellationToken);
            
            if (proposal == null)
            {
                throw new ProposalNotFoundException(request.ProposalId);
            }

            var voteId = await _proposalService.VoteAsync(proposal, request.Result, _currentUser.UserId);

            var e = _eventFactory.CreateEvent(Events.BusinessManagement.Constants.EventPaths.Companies,
                new ProposalVoted(voteId, _currentUser.UserId));
            await _eventPublisher.PublishAsync(e);
            
            return Unit.Value;
        }
    }
}