using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.Exceptions;
using Mosaico.Application.Wallet.Exceptions;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.CommandLine.Base;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Domain.Wallet.Entities.Enums;
using Mosaico.Domain.Wallet.Entities.Staking;
using Mosaico.Domain.Wallet.Exceptions;
using Serilog;

namespace Mosaico.Tools.CommandLine.Commands
{
    // distribute-mosaico-staking -companyId=696489f1-d2c4-47c9-b519-08da1682598e -network=Polygon -stakingPairId=6fb89506-7ba2-4b63-b701-08da5dd5ed5c -amount=632.9 -rewardToken=USDC
    [Command("distribute-mosaico-staking")]
    public class DistributeMosaicoStakingCommand : CommandBase
    {
        private readonly IWalletDbContext _dbContext;
        private readonly ILogger _logger;
        private Guid _companyId;
        private Guid _stakingPairId;
        private string _network;
        private decimal _amount;
        private string _rewardToken;
        private readonly ICompanyWalletService _companyWalletService;
        private readonly IWalletEmailService _emailService;
        private readonly IUserWalletService _userWalletService;
        private readonly IIdentityContext _identityContext;
        private readonly bool _dryRun = false;
        
        public DistributeMosaicoStakingCommand(IWalletDbContext dbContext, ILogger logger, ICompanyWalletService companyWalletService, IUserWalletService userWalletService, IWalletEmailService emailService, IIdentityContext identityContext)
        {
            _dbContext = dbContext;
            _logger = logger;
            _companyWalletService = companyWalletService;
            _userWalletService = userWalletService;
            _emailService = emailService;
            _identityContext = identityContext;
            SetOption("-companyId", "Company id to pay dividend from", s => _companyId = Guid.Parse(s));
            SetOption("-network", "Network of the company wallet", s => _network = s);
            SetOption("-stakingPairId", "Staking Pair ID", s => _stakingPairId = Guid.Parse(s));
            SetOption("-amount", "Amount of reward to distribute", s => _amount = decimal.Parse(s));
            SetOption("-rewardToken", "Ticker of reward token", s => _rewardToken = s);
        }

        public override async Task Execute()
        {
            var now = DateTimeOffset.UtcNow;
            var companyWallet = await _dbContext.CompanyWallets.FirstOrDefaultAsync(t => t.CompanyId == _companyId);
            if (companyWallet == null)
            {
                throw new CompanyWalletNotFoundException(_companyId.ToString());
            }

            var stakingPair = await _dbContext.StakingPairs.FirstOrDefaultAsync(t => t.Id == _stakingPairId);
            if (stakingPair == null)
            {
                throw new StakingPairNotFoundException(_stakingPairId);
            }
            
            var stakings = await _dbContext.Stakings
                .Where(t => t.Status == StakingStatus.Deployed && t.StakingPairId == stakingPair.Id)
                .ToListAsync();
            if (!stakings.Any())
            {
                throw new Exception($"No stakings found");
            }

            var rewardToken =
                await _dbContext.PaymentCurrencies.FirstOrDefaultAsync(t =>
                    t.Chain == _network && t.Ticker == _rewardToken);
            if (rewardToken == null)
            {
                throw new UnsupportedPaymentCurrencyException(_rewardToken);
            }

            var stakeTokenCount = stakings.Sum(staking => staking.Balance);
            var rewardPerTokenPerDay = _amount / stakeTokenCount / 7;

            _logger.Information($"Total amount of staked tokens is {stakeTokenCount}");
            
            var groups = stakings.GroupBy(s => s.UserId);
            var userRewards = new Dictionary<string, decimal>();
            
            foreach (var group in groups)
            {
                var userWallets = group.ToList().GroupBy(t => t.WalletType);
                foreach (var userWallet in userWallets)
                {
                    var wallet = await _dbContext.Wallets.FirstOrDefaultAsync(t => t.UserId == group.Key && t.Network == _network);
                    if (wallet == null) throw new Exception("User wallet not found");
                    foreach (var staking in userWallet)
                    {
                        var daysDifference = (now - staking.CreatedAt).Days;
                        if (daysDifference > 7)
                        {
                            daysDifference = 7;
                        }
                        var stakedAmount = staking.Balance;
                        var reward = rewardPerTokenPerDay * stakedAmount * daysDifference;
                        if (stakedAmount > 0)
                        {
                            var walletAddress = wallet.AccountAddress;
                            if (userWallet.Key == StakingWallet.METAMASK)
                            {
                                walletAddress = staking.Wallet;
                            }
                            if (!userRewards.ContainsKey(walletAddress))
                            {
                                userRewards.Add(walletAddress, 0m);
                            }

                            userRewards[walletAddress] += decimal.Round(reward, 4, MidpointRounding.ToZero);
                        }
                    }
                }
            }
            
            _logger.Information($"There are {userRewards.Count} users to receive a reward");

            var rewardSum = userRewards.Sum(t => t.Value);
            if (rewardSum > _amount)
                throw new Exception(
                    $"Error in calculations. Reward was counted {rewardSum} which is bigger than declared {_amount}");
            
            foreach (var userReward in userRewards.Where(t => t.Value > 0.01m))
            {
                var userWallet = await _dbContext.Wallets.FirstOrDefaultAsync(t => t.AccountAddress == userReward.Key && t.Network == _network);
                var userId = userWallet?.UserId;
                _logger.Information($"User {userReward.Key} is eligible for the reward {userReward.Value}");

                if (userWallet == null)
                {
                    var staking = await _dbContext.Stakings.FirstOrDefaultAsync(t => t.Wallet == userReward.Key);
                    userId = staking?.UserId;
                }
                
                var claimThreshold = now.AddDays(-7);
                
                var claimHistory = await _dbContext.StakingClaimHistory.FirstOrDefaultAsync(t =>
                    t.UserId == userId && t.StakingPairId == stakingPair.Id && t.ClaimedAt >= claimThreshold && t.Wallet == userReward.Key);
                if (claimHistory != null)
                {
                    _logger.Warning($"User {userReward.Key} already received a reward. Skipping...");
                    continue;
                }
                
                if (!_dryRun)
                {
                    _logger.Information($"Starting sending the reward {userReward.Value} {rewardToken.Ticker} to {userReward.Key}");
                    var transactionHash = await _companyWalletService.TransferTokenWithoutReceiptAsync(companyWallet, rewardToken.ContractAddress,
                        userReward.Key, userReward.Value);
                    claimHistory = new StakingClaimHistory
                    {
                        Amount = userReward.Value,
                        StakingPair = stakingPair,
                        StakingPairId = stakingPair.Id,
                        ClaimedAt = now,
                        UserId = userId,
                        Wallet = userReward.Key,
                        TransactionHash = transactionHash
                    };
                    await _dbContext.StakingClaimHistory.AddAsync(claimHistory);
                    await _dbContext.SaveChangesAsync();
                    _logger.Information($"Reward was paid via transaction {transactionHash}");
                    var user = await _identityContext.Users.FirstOrDefaultAsync(t => t.Id == userId);
                    if (user != null)
                    {
                        _logger.Information($"Sending email to {user.Email}");
                        await _emailService.SendStakingRewardPaid(stakingPair.Token, user.Email, userReward.Value,
                            rewardToken, user.Language);
                    }
                }
            }

            Console.ReadLine();
        }
    }
}