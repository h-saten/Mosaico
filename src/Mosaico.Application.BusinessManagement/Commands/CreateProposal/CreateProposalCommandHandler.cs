using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Authorization.Base;
using Mosaico.Base.Tools;
using Mosaico.Blockchain.Base.DAL;
using Mosaico.Blockchain.Base.Extensions;
using Mosaico.Domain.BusinessManagement.Abstractions;
using Mosaico.Domain.BusinessManagement.Entities;
using Mosaico.Domain.BusinessManagement.Exceptions;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.BusinessManagement;
using Mosaico.SDK.Wallet.Abstractions;

namespace Mosaico.Application.BusinessManagement.Commands.CreateProposal
{
    public class CreateProposalCommandHandler : IRequestHandler<CreateProposalCommand, Guid>
    {
        private readonly IBusinessDbContext _businessDb;
        private readonly IWalletClient _walletClient;
        private readonly IAccountRepository _accountRepository;
        private readonly ICurrentUserContext _currentUser;
        private readonly IEventPublisher _eventPublisher;
        private readonly IDateTimeProvider _provider;
        private readonly IEventFactory _eventFactory;

        public CreateProposalCommandHandler(IBusinessDbContext businessDb, IWalletClient walletClient, IAccountRepository accountRepository, ICurrentUserContext currentUser, IEventPublisher eventPublisher, IEventFactory eventFactory, IDateTimeProvider provider)
        {
            _businessDb = businessDb;
            _walletClient = walletClient;
            _accountRepository = accountRepository;
            _currentUser = currentUser;
            _eventPublisher = eventPublisher;
            _eventFactory = eventFactory;
            _provider = provider;
        }

        public async Task<Guid> Handle(CreateProposalCommand request, CancellationToken cancellationToken)
        {
            var company = await _businessDb.Companies.FirstOrDefaultAsync(t => t.Id == request.CompanyId, cancellationToken);
            if (company == null)
            {
                throw new CompanyNotFoundException(request.CompanyId);
            }

            var token = await _walletClient.GetTokenAsync(request.TokenId);
            if (token == null)
            {
                throw new TokenNotFoundException(request.TokenId);
            }
            
            var companyWallet = await _walletClient.GetCompanyWalletAsync(request.CompanyId, token.Network);
            if (!companyWallet.Tokens.Any(t => t.Id == token.Id))
            {
                throw new TokenNotFoundException(request.TokenId);
            }

            if (!company.IsVotingEnabled || !company.TeamMembers.Any(t => t.UserId == _currentUser.UserId) && !company.OnlyOwnerProposals)
            {
                throw new UnauthorizedProposalException();
            }

            var userWallet = await _walletClient.GetUserWalletAsync(_currentUser.UserId, token.Network);
            if (userWallet == null)
            {
                throw new WalletNotFoundException(_currentUser.UserId, token.Network);
            }
            
            var tokenBalance = (await _accountRepository.Erc20BalanceAsync(userWallet.AccountAddress, token.Address, token.Network)).ConvertToDecimal();
            if (tokenBalance <= 0)
            {
                throw new UnauthorizedProposalException();
            }

            var startDate = _provider.Now().AddMinutes(Constants.ProposalThresholdInMinutes).Date;
            var endDate = startDate.AddMinutes(Constants.ProposalLifecycleInMinutes);

            var proposalId = company.Proposals.Count + 1;
            
            var proposal = new Proposal
            {
                Company = company,
                CompanyId = company.Id,
                Network = token.Network,
                TokenId = token.Id,
                QuorumThreshold = request.QuorumThreshold,
                CreatedByAddress = userWallet.AccountAddress,
                Title = request.Title.Trim(),
                StartsAt = startDate,
                EndsAt = endDate,
                Description = request.Description.Trim(),
                ProposalId = proposalId.ToString()
            };
            _businessDb.Proposals.Add(proposal);
            await _businessDb.SaveChangesAsync(cancellationToken);
            await PublishEventAsync(proposal.Id);
            return proposal.Id;
        }

        private async Task PublishEventAsync(Guid id)
        {
            await _eventPublisher.PublishAsync(_eventFactory.CreateEvent(
                Events.BusinessManagement.Constants.EventPaths.Companies, new ProposalCreatedEvent(id, _currentUser.UserId)));
        }
    }
}