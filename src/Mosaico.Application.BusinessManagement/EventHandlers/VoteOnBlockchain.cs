using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.BusinessManagement.Abstractions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.BusinessManagement;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.Integration.Blockchain.Ethereum.Models;
using Mosaico.Integration.SignalR.Abstractions;

namespace Mosaico.Application.BusinessManagement.EventHandlers
{
    [EventInfo(nameof(VoteOnBlockchain), "companies:api")]
    [EventTypeFilter(typeof(ProposalVoted))]
    public class VoteOnBlockchain : EventHandlerBase
    {
        private readonly IBusinessDbContext _businessDbContext;
        private readonly IDaoService _daoService;
        private readonly IDaoDispatcher _daoDispatcher;
        private readonly IWalletDbContext _walletDbContext;
        private readonly IEthereumClientFactory _ethereumClientFactory;

        public VoteOnBlockchain(IBusinessDbContext businessDbContext, IDaoService daoService, IDaoDispatcher daoDispatcher, IWalletDbContext walletDbContext, IEthereumClientFactory ethereumClientFactory)
        {
            _businessDbContext = businessDbContext;
            _daoService = daoService;
            _daoDispatcher = daoDispatcher;
            _walletDbContext = walletDbContext;
            _ethereumClientFactory = ethereumClientFactory;
        }

        public override async Task HandleAsync(CloudEvent @event)
        {
            var data = @event?.GetData<ProposalVoted>();
            if (data != null)
            {
                using (var transaction = _businessDbContext.BeginTransaction())
                {
                    var vote = await _businessDbContext.Votes.Include(v => v.Proposal).ThenInclude(p => p.Company)
                        .FirstOrDefaultAsync(v => v.Id == data.Id);
                    if (vote != null)
                    {
                        try
                        {
                            var userWallet = await _walletDbContext.Wallets.FirstOrDefaultAsync(w =>
                                w.Network == vote.Proposal.Network && w.UserId == data.UserId);
                            if (userWallet == null)
                            {
                                throw new WalletNotFoundException(data.UserId);
                            }

                            //TODO: temprorary
                            
                            var companyWallet = await _walletDbContext.CompanyWallets.FirstOrDefaultAsync(c =>
                                c.Network == vote.Proposal.Network && c.CompanyId == vote.Proposal.CompanyId);
                            
                            var client = _ethereumClientFactory.GetClient(vote.Proposal.Network);
                            var account = await client.GetAccountAsync(companyWallet.PrivateKey);
                            await client.TransferFundsAsync(account, userWallet.AccountAddress, 0.01m);
                            
                            var transactionHash = await _daoService.VoteAsync(vote.Proposal.Network,
                                new Daov1Configurations.VoteConfiguration
                                {
                                    DaoAddress = vote.Proposal.Company.ContractAddress,
                                    ProposalId = vote.Proposal.ProposalId,
                                    Result = (VoteResult)(int)vote.Result,
                                    PrivateKey = userWallet.PrivateKey
                                });
                            vote.TransactionHash = transactionHash;
                            await _businessDbContext.SaveChangesAsync();
                            await transaction.CommitAsync();
                            await _daoDispatcher.VoteSubmitted(data.UserId, transactionHash);
                        }
                        catch (Exception ex)
                        {
                            await transaction.RollbackAsync();
                            _businessDbContext.Votes.Remove(vote);
                            await _businessDbContext.SaveChangesAsync();
                            await _daoDispatcher.VoteFailed(data.UserId, ex.Message);
                        }
                    }
                }
            }
        }
    }
}