using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Abstractions;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Domain.Wallet.Entities.Enums;
using Mosaico.Domain.Wallet.Entities.Staking;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.SDK.Relay.Abstractions;
using Mosaico.SDK.Relay.Models;
using Nethereum.RPC.Accounts;
using Serilog;

namespace Mosaico.Application.Wallet.Services
{
    public class WalletStakingService : IWalletStakingService
    {
        private readonly IStakingService _stakingService;
        private readonly IUserWalletService _userWalletService;
        private readonly ICompanyWalletService _companyWalletService;
        private readonly IEthereumClientFactory _ethereumClientFactory;
        private readonly ITokenService _tokenService;
        private readonly IWalletDbContext _context;
        private readonly IMosaicoRelayClient _relayClient;
        private readonly ILogger _logger;

        public WalletStakingService(IStakingService stakingService, IWalletDbContext context, ILogger logger, IEthereumClientFactory ethereumClientFactory, ITokenService tokenService, IUserWalletService userWalletService, ICompanyWalletService companyWalletService, IMosaicoRelayClient relayClient)
        {
            _stakingService = stakingService;
            _context = context;
            _logger = logger;
            _ethereumClientFactory = ethereumClientFactory;
            _tokenService = tokenService;
            _userWalletService = userWalletService;
            _companyWalletService = companyWalletService;
            _relayClient = relayClient;
        }

        public async Task<bool> CanClaimAsync(Staking stake)
        {
            var anotherOperation = await _context.Operations
                .FirstOrDefaultAsync(t => t.Network == stake.StakingPair.Network && t.Type == BlockchainOperationType.STAKE_CLAIMING &&
                                          t.State == OperationState.IN_PROGRESS
                                          && t.UserId == stake.UserId);
            if (anotherOperation != null)
            {
                _logger?.Warning($"User {stake.UserId} runs another staking operation.");
                return false;
            }
            
            var now = DateTimeOffset.UtcNow;
            var lastClaim = await _context.StakingClaimHistory
                .Where(c => c.UserId == stake.UserId && c.StakingPairId == stake.StakingPairId)
                .OrderByDescending(c => c.ClaimedAt).FirstOrDefaultAsync();
            if (lastClaim != null)
            {
                DateTimeOffset currentCycleStartDate;
                DateTimeOffset currentCycleEndDate;
                
                if (now.Day > stake.StakingPair.RewardPayedOnDay || (now.Day == stake.StakingPair.RewardPayedOnDay && now.Hour > 12))
                {
                    currentCycleStartDate = new DateTimeOffset(now.Year, now.Month, stake.StakingPair.RewardPayedOnDay, 12, 0, 0, TimeSpan.Zero);
                    currentCycleEndDate = currentCycleStartDate.AddMonths(1);
                }
                else
                {
                    currentCycleEndDate = new DateTimeOffset(now.Year, now.Month, stake.StakingPair.RewardPayedOnDay, 12, 0, 0, TimeSpan.Zero);
                    currentCycleStartDate = currentCycleEndDate.AddMonths(-1);
                }

                if (lastClaim.ClaimedAt >= currentCycleStartDate && lastClaim.ClaimedAt <= currentCycleEndDate)
                {
                    return false;
                }
            }

            return true;
        }

        public async Task<bool> CanStakeAsync(Staking stake)
        {
            if (!stake.CanDeploy())
            {
                _logger?.Warning($"Staking {stake.Id} has wrong status. Expected: {StakingStatus.Pending}. Got {stake.Status}");
                return false;
            }
            
            var anotherOperation = await _context.Operations
                .FirstOrDefaultAsync(t => t.Network == stake.StakingPair.Network && t.Type == BlockchainOperationType.STAKING &&
                                          t.State == OperationState.IN_PROGRESS && t.TransactionId == stake.StakingPairId
                                          && t.UserId == stake.UserId);
            
            if (anotherOperation != null)
            {
                _logger?.Warning($"User {stake.UserId} runs another staking operation.");
                return false;
            }
            
            return true;
        }

        private async Task<Domain.Wallet.Entities.Wallet> GetUserWalletAsync(Staking stake)
        {
            var userWallet = await _context.Wallets.FirstOrDefaultAsync(w =>
                w.Network == stake.StakingPair.Network && w.UserId == stake.UserId);
                
            if (userWallet == null) throw new WalletNotFoundException(stake.UserId);
            return userWallet;
        }

        private async Task TransferNativeCurrencyIfRequiredAsync(Staking stake, decimal valueToTransfer = 0.01m)
        {
            var userWallet = await GetUserWalletAsync(stake);
            
            var nativeCurrency = await _context.PaymentCurrencies.FirstOrDefaultAsync(c =>
                c.NativeChainCurrency && c.Chain == stake.StakingPair.Network);
            
            if (nativeCurrency == null)
                throw new Exception($"Cannot find native currency for network {stake.StakingPair.Network}");
            
            var balance = await _userWalletService.NativePaymentCurrencyBalanceAsync(userWallet.AccountAddress, nativeCurrency.Ticker, userWallet.Network);
            if (balance <= valueToTransfer)
            {
                var client = _ethereumClientFactory.GetClient(userWallet.Network);
                await client.TransferFundsAsync(userWallet.AccountAddress, valueToTransfer);
            }
        }

        private async Task<IAccount> GetAccountAsync(Staking stake)
        {
            var userWallet = await GetUserWalletAsync(stake);
            var client = _ethereumClientFactory.GetClient(stake.StakingPair.Network);
            var account = await client.GetAccountAsync(userWallet.PrivateKey);
            return account;
        }

        private string GetPaymentCurrencyContractAddress(Staking stake)
        {
            var contractAddress = stake.StakingPair.StakingToken?.Address;
            if (stake.StakingPair.Type == StakingPairBaseCurrencyType.Currency)
            {
                contractAddress = stake.StakingPair.StakingPaymentCurrency.ContractAddress;
            }
            return contractAddress;
        }

        public async Task<bool> RequiresApprovalAsync(Staking stake)
        {
            var account = await GetUserWalletAsync(stake);
            var contractAddress = GetPaymentCurrencyContractAddress(stake);
            if (stake.StakingPair.StakingVersion == "milky_v1")
            {
                var allowance = await _relayClient.AllowanceAsync(new AllowanceParams
                {
                    Owner = account.AccountAddress,
                    Spender = stake.StakingPair.ContractAddress,
                    UserId = stake.UserId
                });
                return allowance < stake.Balance;
            }
            else
            {
                var allowance = await _tokenService.GetAllowanceAsync(stake.StakingPair.Network, config =>
                {
                    config.ContractAddress = contractAddress;
                    config.SpenderAddress = stake.StakingPair.ContractAddress;
                    config.OwnerPrivateKey = account.PrivateKey;
                    config.OwnerAddress = account.AccountAddress;
                });
                return allowance < stake.Balance;
            }
        }

        public async Task<string> ApproveAsync(Staking stake)
        {
            var account = await GetAccountAsync(stake);
            var contractAddress = GetPaymentCurrencyContractAddress(stake);
            
            await TransferNativeCurrencyIfRequiredAsync(stake, 0.02m);
            
            var approvalTransactionHash = await _tokenService.ApproveAsync(stake.StakingPair.Network, account, contractAddress,
                stake.StakingPair.ContractAddress, stake.Balance);
                
            return approvalTransactionHash;
        }
        
        public async Task<string> DistributeAsync(StakingPair pair, decimal amount, string ownerPrivateKey)
        {
            //TODO: revisit
            var contractAddress = pair.StakingToken?.Address;
            if (pair.Type == StakingPairBaseCurrencyType.Currency)
            {
                contractAddress = pair.StakingPaymentCurrency.ContractAddress;
            }
            
            var decimals = 6;

            var hash = await _stakingService.DistributeAsync(pair.Network, c =>
            {
                c.Amount = amount;
                c.StakingAddress = pair.ContractAddress;
                c.RewardTokenAddress = contractAddress;
                c.StakerPrivateKey = ownerPrivateKey;
                c.Decimals = decimals;
            });
                
            return hash;
        }
        
        public async Task<string> StakeAsync(Staking stake)
        {
            var userWallet = await GetUserWalletAsync(stake);
            if (stake.StakingPair.StakingVersion == "milky_v1")
            {
                var transactionHash = await _relayClient.StakeAsync(new StakeParams
                {
                    Amount = stake.Balance,
                    Network = stake.StakingPair.Network,
                    UserId = stake.UserId,
                    StakingPairId = stake.StakingPairId
                });
                return transactionHash;
            }
            else
            {
                await TransferNativeCurrencyIfRequiredAsync(stake);
                var transactionHash = await _stakingService.StakeAsync(stake.StakingPair.Network, c =>
                {
                    c.Amount = stake.Balance;
                    c.StakingAddress = stake.StakingPair.ContractAddress;
                    c.StakerPrivateKey = userWallet.PrivateKey;
                });
                return transactionHash;
            }
        }

        public async Task<string> ApproveWithdrawableTokenAsync(Staking stake, decimal balance)
        {
            var tokenAddress = stake.StakingPair.Token?.Address;
            if (string.IsNullOrWhiteSpace(tokenAddress)) throw new TokenNotFoundException(stake.StakingPairId);
            await TransferNativeCurrencyIfRequiredAsync(stake);
            var account = await GetAccountAsync(stake);
            var hash =  await _tokenService.ApproveAsync(stake.StakingPair.Network, account, tokenAddress, stake.StakingPair.ContractAddress, balance);
            return hash;
        }

        public async Task<string> WithdrawAsync(Staking stake)
        {
            if (stake.StakingPair.StakingVersion == "milky_v1")
            {
                var transactionHash = await _relayClient.Withdraw(new WithdrawParams
                {
                    Network = stake.StakingPair.Network,
                    UserId = stake.UserId,
                    StakingPairId = stake.StakingPairId
                });
                return transactionHash;
            }
            else
            {
                var userWallet = await GetUserWalletAsync(stake);
                await TransferNativeCurrencyIfRequiredAsync(stake);
                var transactionHash = await _stakingService.WithdrawAsync(stake.StakingPair.Network, c =>
                {
                    c.StakingAddress = stake.StakingPair.ContractAddress;
                    c.StakerPrivateKey = userWallet.PrivateKey;
                });
                return transactionHash;
            }
        }

        private async Task<string> ClaimManuallyAsync(Staking stake)
        {
            var userWallet = await GetUserWalletAsync(stake);
            var companyId = stake.StakingPair.StakingToken.CompanyId;
            var companyWallet = await _context.CompanyWallets.FirstOrDefaultAsync(c => c.CompanyId == companyId && c.Network == stake.StakingPair.Network);
            var paymentCurrencyContractAddress = stake.StakingPair.PaymentCurrencies.FirstOrDefault()?.PaymentCurrency?.ContractAddress;
            var claimableBalance = await GetEstimatedRewardAsync(stake);
            if (claimableBalance <= 0) throw new Exception($"Insufficient funds to claim");
            var transactionHash = await _companyWalletService.TransferTokenWithoutReceiptAsync(companyWallet, paymentCurrencyContractAddress, userWallet.AccountAddress, claimableBalance);
            return transactionHash;
        }

        public async Task<string> ClaimAsync(Staking stake)
        {
            if (stake.StakingPair.StakingVersion == "RC")
            {
                var tx =  await ClaimManuallyAsync(stake);
                return tx;
            }
            else if (stake.StakingPair.StakingVersion == "milky_v1")
            {
                var transactionHash = await _relayClient.ClaimAsync(new ClaimParams
                {
                    Network = stake.StakingPair.Network,
                    UserId = stake.UserId,
                    StakingPairId = stake.StakingPairId
                });
                return transactionHash;
            }
            else
            {

                var contract = GetPaymentCurrencyContractAddress(stake);
                var userWallet = await GetUserWalletAsync(stake);
                await TransferNativeCurrencyIfRequiredAsync(stake);
                var transactionHash = await _stakingService.ClaimAsync(stake.StakingPair.Network, c =>
                {
                    c.StakingAddress = stake.StakingPair.ContractAddress;
                    c.StakerPrivateKey = userWallet.PrivateKey;
                    c.TokenAddress = contract;
                });
                return transactionHash;
            }
        }

        public async Task<decimal> GetEstimatedRewardAsync(Staking stake)
        {
            var rewardCurrencyAddress = "";
            var paymentCurrencyId = stake.StakingPair.PaymentCurrencies?.FirstOrDefault()?.PaymentCurrencyId;
            if (paymentCurrencyId.HasValue)
            {
                var paymentCurrency = await _context.PaymentCurrencies.FirstOrDefaultAsync(t => t.Id == paymentCurrencyId.Value);
                rewardCurrencyAddress = paymentCurrency?.ContractAddress;
            }
            var userWallet = await GetUserWalletAsync(stake);
            var balance = await _stakingService.GetClaimableAmountAsync(stake.StakingPair.Network, c =>
            {
                c.StakingAddress = stake.StakingPair.ContractAddress;
                c.StakerPrivateKey = userWallet.PrivateKey;
                c.Wallet = userWallet.AccountAddress;
                c.RewardTokenAddress = rewardCurrencyAddress;
            }); 
            return balance;
        }

        public async Task SetDeploymentCompleted(Staking stake)
        {
            stake.Status = StakingStatus.Deployed;
            _context.Stakings.Update(stake);
            await _context.SaveChangesAsync();
        }
        
        public async Task SetDeploymentFailed(Staking stake, string error)
        {
            stake.Status = StakingStatus.Failed;
            stake.FailureReason = error;
            _context.Stakings.Update(stake);
            await _context.SaveChangesAsync();
        }

        public async Task StartDeploymentAsync(Staking stake)
        {
            stake.Status = StakingStatus.Deploying;
            _context.Stakings.Update(stake);
            await _context.SaveChangesAsync();
        }

        public async Task<decimal> GetStakingBalanceAsync(Staking stake)
        {
            var userWallet = await GetUserWalletAsync(stake);
            var balance = await _stakingService.GetBalanceOfAsync(stake.StakingPair.Network, stake.StakingPair.ContractAddress, userWallet.AccountAddress); 
            return balance;
        }
        
        public async Task<decimal> GetStakingBalanceAsync(StakingPair pair, string userWallet)
        {
            var balance = await _stakingService.GetBalanceOfAsync(pair.Network, pair.ContractAddress, userWallet); 
            return balance;
        }
    }
}