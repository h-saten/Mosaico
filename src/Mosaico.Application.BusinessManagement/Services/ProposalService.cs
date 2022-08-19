using System;
using System.Linq;
using System.Threading.Tasks;
using Mosaico.Blockchain.Base.DAL;
using Mosaico.Blockchain.Base.Extensions;
using Mosaico.Domain.BusinessManagement.Abstractions;
using Mosaico.Domain.BusinessManagement.Entities;
using Mosaico.Domain.BusinessManagement.Exceptions;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.SDK.Wallet.Abstractions;

namespace Mosaico.Application.BusinessManagement.Services
{
    public class ProposalService : IProposalService
    {
        private readonly IEthereumClientFactory _ethereumClientFactory;
        private readonly IAccountRepository _accountRepository;
        private readonly IWalletClient _walletClient;
        private readonly IBusinessDbContext _businessDb;

        public ProposalService(IEthereumClientFactory ethereumClientFactory, IAccountRepository accountRepository, IWalletClient walletClient, IBusinessDbContext businessDb)
        {
            _ethereumClientFactory = ethereumClientFactory;
            _accountRepository = accountRepository;
            _walletClient = walletClient;
            _businessDb = businessDb;
        }

        public async Task<Guid> VoteAsync(Proposal proposal, VoteResult result, string userId)
        {
            var token = await _walletClient.GetTokenAsync(proposal.TokenId);
            if (token == null)
            {
                throw new TokenNotFoundException(proposal.TokenId);
            }

            var userWallet = await _walletClient.GetUserWalletAsync(userId, token.Network);
            if (userWallet == null)
            {
                throw new WalletNotFoundException(Guid.Parse(userId), token.Network);
            }

            var tokenBalance =
                (await _accountRepository.Erc20BalanceAsync(userWallet.AccountAddress, token.Address, token.Network)).ConvertToDecimal();
            
            if (proposal.Votes.Any(v => v.VotedByAddress == userWallet.AccountAddress))
            {
                throw new AlreadyVotedException(userWallet.AccountAddress);
            }

            if (proposal.GetStatus() != VotingStatus.Active)
            {
                throw new UnauthorizedVoteException("Proposal in inactive");
            }

            if (tokenBalance <= 0)
            {
                throw new UnauthorizedVoteException("Insufficient tokens");
            }

            var vote = new Vote
            {
                Result = result,
                VotedByAddress = userWallet.AccountAddress,
                Tokens = tokenBalance,
                ProposalId = proposal.Id,
                Proposal = proposal
            };
            _businessDb.Votes.Add(vote);
            await _businessDb.SaveChangesAsync();
            return vote.Id;
        }
    }
}