using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.BusinessManagement.Abstractions;
using Mosaico.Domain.BusinessManagement.Exceptions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.BusinessManagement;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.Integration.SignalR.Abstractions;
using Mosaico.SDK.Wallet.Abstractions;

namespace Mosaico.Application.BusinessManagement.EventHandlers
{
    [EventInfo(nameof(CreateProposalOnBlockchain), "companies:api")]
    [EventTypeFilter(typeof(ProposalCreatedEvent))]
    public class CreateProposalOnBlockchain : EventHandlerBase
    {
        private readonly IBusinessDbContext _businessDbContext;
        private readonly IDaoService _daoService;
        private readonly IDaoDispatcher _daoDispatcher;
        private readonly IWalletClient _walletClient;
        private readonly IWalletDbContext _walletDbContext;
        
        public CreateProposalOnBlockchain(IBusinessDbContext businessDbContext, IDaoService daoService, IDaoDispatcher daoDispatcher, IWalletClient walletClient, IWalletDbContext walletDbContext)
        {
            _businessDbContext = businessDbContext;
            _daoService = daoService;
            _daoDispatcher = daoDispatcher;
            _walletClient = walletClient;
            _walletDbContext = walletDbContext;
        }

        public override async Task HandleAsync(CloudEvent @event)
        {
            var eventData = @event?.GetData<ProposalCreatedEvent>();
            if (eventData != null)
            {
                using (var transaction = _businessDbContext.BeginTransaction())
                {
                    var proposal = await _businessDbContext.Proposals.Include(p => p.Company)
                                            .FirstOrDefaultAsync(p => p.Id == eventData.Id);
                    if (proposal == null) return;
                    try
                    {
                        
                        var token = await _walletClient.GetTokenAsync(proposal.TokenId);
                        if (token == null || string.IsNullOrWhiteSpace(token.Address))
                        {
                            throw new TokenNotFoundException(proposal.TokenId);
                        }

                        if (proposal.Company == null || string.IsNullOrWhiteSpace(proposal.Company.ContractAddress))
                        {
                            throw new CompanyNotFoundException(proposal.CompanyId);
                        }
                        
                        if (!proposal.DeployedAt.HasValue)
                        {
                            var userWallet = await _walletDbContext.Wallets.FirstOrDefaultAsync(w =>
                                w.Network == proposal.Network && w.UserId == eventData.UserId);
                            if (userWallet == null)
                            {
                                throw new WalletNotFoundException(eventData.UserId);
                            }
                            
                            var proposalId = await _daoService.CreateProposalAsync(proposal.Network,
                                new Daov1Configurations.CreateProposalConfiguration
                                {
                                    Description = proposal.Description,
                                    ContractAddress = token.Address,
                                    DaoAddress = proposal.Company.ContractAddress,
                                    PrivateKey = userWallet.PrivateKey
                                });
                            proposal.ProposalId = proposalId;
                            proposal.DeployedAt = DateTimeOffset.UtcNow;
                            await _businessDbContext.SaveChangesAsync();
                            await _daoDispatcher.ProposalCreated(eventData.UserId, proposalId);
                        }

                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        _businessDbContext.Proposals.Remove(proposal);
                        await _businessDbContext.SaveChangesAsync();
                        await _daoDispatcher.ProposalCreationFailed(eventData.UserId, ex.Message);
                    }
                }
            }
        }
    }
}