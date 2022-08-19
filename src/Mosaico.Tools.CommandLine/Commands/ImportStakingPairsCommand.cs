using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Exceptions;
using Mosaico.CommandLine.Base;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Domain.Wallet.Entities.Enums;
using Mosaico.Domain.Wallet.Entities.Staking;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.Tools.CommandLine.Models;
using Newtonsoft.Json;

namespace Mosaico.Tools.CommandLine.Commands
{
    [Command("import-staking-pair")]
    public class ImportStakingPairsCommand : CommandBase
    {
        private readonly IWalletDbContext _walletDbContext;

        public ImportStakingPairsCommand(IWalletDbContext walletDbContext)
        {
            _walletDbContext = walletDbContext;
        }

        public override async Task Execute()
        {
            var fileLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "stakingpairs.json");
            if (!File.Exists(fileLocation))
            {
                throw new Exception($"File {fileLocation} does not exist");
            }
            var file = await File.ReadAllTextAsync(fileLocation, Encoding.UTF8);
            var stakingPairs = JsonConvert.DeserializeObject<List<StakingPairImportDTO>>(file);
            foreach (var stakingPair in stakingPairs)
            {
                var token = await _walletDbContext.Tokens.FirstOrDefaultAsync(t => t.Symbol == stakingPair.Token && t.Network == stakingPair.Network);
                var stakingToken = await _walletDbContext.Tokens.FirstOrDefaultAsync(t => t.Symbol == stakingPair.StakingToken && t.Network == stakingPair.Network);
                var stakingCurrency = await _walletDbContext.PaymentCurrencies.FirstOrDefaultAsync(t =>
                    t.Ticker == stakingPair.StakingToken && t.Chain == stakingPair.Network);
                if(token == null) throw new TokenNotFoundException();
                if(stakingToken == null && stakingPair.Type == StakingPairBaseCurrencyType.Token) throw new TokenNotFoundException();
                if(stakingCurrency == null && stakingPair.Type == StakingPairBaseCurrencyType.Currency) throw new UnsupportedCurrencyException(stakingPair.StakingToken);
                
                var spInDb = await _walletDbContext.StakingPairs.FirstOrDefaultAsync(t =>
                    t.Network == stakingPair.Network && t.TokenId == token.Id
                                                     && (stakingPair.Type != StakingPairBaseCurrencyType.Currency || t.StakingPaymentCurrencyId == stakingCurrency.Id)
                                                     && (stakingPair.Type != StakingPairBaseCurrencyType.Token || t.StakingTokenId == stakingToken.Id));
                if (spInDb == null)
                {
                    spInDb = new StakingPair
                    {
                        Network = stakingPair.Network,
                        Token  = token,
                        TokenId = token.Id,
                        ContractAddress = stakingPair.Address,
                        IsEnabled = true,
                        StakingToken = stakingToken,
                        StakingTokenId = stakingToken?.Id,
                        StakingPaymentCurrency = stakingCurrency,
                        StakingPaymentCurrencyId = stakingCurrency?.Id,
                        CanChangeStakingPeriod = stakingPair.CanChangeStakingPeriod,
                        EstimatedAPR = stakingPair.EstimatedAPR,
                        EstimatedRewardInUSD = stakingPair.EstimatedRewardInUSD,
                        MinimumDaysToStake = stakingPair.MinimumDaysToStake,
                        RewardPayedOnDay = stakingPair.RewardPayedOnDay,
                        Type = stakingPair.Type,
                        CronSchedule = stakingPair.Cron,
                        StakingVersion = stakingPair.Version,
                        SkipApproval = stakingPair.SkipApproval
                    };
                    await _walletDbContext.StakingPairs.AddAsync(spInDb);
                }
                else
                {
                    spInDb.IsEnabled = true;
                    spInDb.ContractAddress = stakingPair.Address;
                    spInDb.CanChangeStakingPeriod = stakingPair.CanChangeStakingPeriod;
                    spInDb.EstimatedAPR = stakingPair.EstimatedAPR;
                    spInDb.EstimatedRewardInUSD = stakingPair.EstimatedRewardInUSD;
                    spInDb.MinimumDaysToStake = stakingPair.MinimumDaysToStake;
                    spInDb.RewardPayedOnDay = stakingPair.RewardPayedOnDay;
                    spInDb.Type = stakingPair.Type;
                    spInDb.CronSchedule = stakingPair.Cron;
                    spInDb.StakingVersion = stakingPair.Version;
                    spInDb.SkipApproval = stakingPair.SkipApproval;
                }
                spInDb.PaymentCurrencies.Clear();
                foreach (var currency in stakingPair.PaymentCurrencies)
                {
                    var paymentCurrency =
                        await _walletDbContext.PaymentCurrencies.FirstOrDefaultAsync(t =>
                            t.Ticker == currency && t.Chain == stakingPair.Network);
                    if (paymentCurrency == null) throw new UnsupportedCurrencyException(currency);
                    spInDb.PaymentCurrencies.Add(new PaymentCurrencyToStakingPair
                    {
                        PaymentCurrency = paymentCurrency,
                        PaymentCurrencyId = paymentCurrency.Id,
                        StakingPair = spInDb,
                        StakingPairId = spInDb.Id
                    });
                }
                await _walletDbContext.SaveChangesAsync();
            }
        }
    }
}