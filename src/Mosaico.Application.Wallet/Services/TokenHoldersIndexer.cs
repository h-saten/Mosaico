using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Blockchain.Base.DAL;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using NBitcoin;
using Serilog;

namespace Mosaico.Application.Wallet.Services
{
    public class TokenHoldersIndexer : ITokenHoldersIndexer
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly ITokenRepository _tokenRepository;
        private readonly ILogger _logger;
        private readonly IEthereumClientFactory _ethereumClient;
        
        private const ulong MaxBlocksRange = 3500;
        private const ulong BlocksRangeSlippage = 10;

        public TokenHoldersIndexer(
            IWalletDbContext walletDbContext, 
            ITokenRepository tokenRepository,  IEthereumClientFactory ethereumClient, ILogger logger = null)
        {
            _walletDbContext = walletDbContext;
            _tokenRepository = tokenRepository;
            _ethereumClient = ethereumClient;
            _logger = logger;
        }

        public async Task<Dictionary<string, decimal>> UpdateHoldersAsync(Guid tokenId, CancellationToken cancellationToken = new())
        {
            var token = await _walletDbContext
                .Tokens
                .Where(x => x.Id == tokenId)
                .SingleOrDefaultAsync(cancellationToken);
            
            var jobLog = await _walletDbContext
                .TokenHolderScanJobLogs
                .Where(x => x.TokenId == token.Id)
                .SingleOrDefaultAsync(cancellationToken);
            
            var (startFromBlockNumber, endBlockNumber) = await GetBlockRange(jobLog, token.Network);

            var transfers = await _tokenRepository
                .Erc20TransfersAsync(token.Address, token.Network, startFromBlockNumber, endBlockNumber);

            var lastBlock = transfers.OrderBy(x => x.BlockNumber).Select(x => x.BlockNumber).LastOrDefault();

            var balances = new Dictionary<string, decimal>();
            
            foreach (var transfer in transfers)
            {
                var currentReceiverBalance = decimal.Zero;
                var currentSenderBalance = decimal.Zero;
                
                if (balances.ContainsKey(transfer.ToAddress))
                {
                    currentReceiverBalance = balances[transfer.ToAddress];
                }
                currentReceiverBalance += transfer.Value;
                
                if (balances.ContainsKey(transfer.FromAddress))
                {
                    currentSenderBalance = balances[transfer.FromAddress];
                }

                currentSenderBalance -= transfer.Value;

                if (transfer.ToAddress != Integration.Blockchain.Ethereum.Constants.DefaultWalletAddress)
                {
                    balances.AddOrReplace(transfer.ToAddress, currentReceiverBalance);
                }

                if (transfer.FromAddress != Integration.Blockchain.Ethereum.Constants.DefaultWalletAddress)
                {
                    balances.AddOrReplace(transfer.FromAddress, currentSenderBalance);
                }

            }

            var holdersAddresses = balances.Keys;
            
            await using var dbTransaction = _walletDbContext.BeginTransaction();

            if (jobLog is not null)
            {
                _walletDbContext.TokenHolderScanJobLogs.Update(jobLog);
            }
            else
            {
                jobLog = new TokenHolderScanJobLog
                {
                    TokenId = token.Id,
                    LastFetchedBlock = (ulong) lastBlock
                };
                await _walletDbContext.TokenHolderScanJobLogs.AddAsync(jobLog, cancellationToken);
            }
            
            try
            {
                // TODO refactor to better performance (batching)
                var holders = await _walletDbContext
                    .TokenHolders
                    .Where(x => x.TokenId == token.Id && holdersAddresses.Contains(x.WalletAddress))
                    .ToListAsync(cancellationToken);

                foreach (var balance in balances)
                {
                    var holder = holders.FirstOrDefault(x => x.WalletAddress == balance.Key);
                    if (holder is null)
                    {
                        holder = new TokenHolder
                        {
                            TokenId = token.Id,
                            Balance = balance.Value,
                            WalletAddress = balance.Key
                        };

                        _walletDbContext.TokenHolders.Add(holder);
                    }
                    else
                    {
                        holder.Balance += balance.Value;
                    }
                }

                await _walletDbContext.SaveChangesAsync(cancellationToken);
                await dbTransaction.CommitAsync(cancellationToken);
            }
            catch (Exception exception)
            {
                _logger?.Error($"Token holders update error. {exception.Message}");
                await dbTransaction.RollbackAsync(cancellationToken);
            }

            return balances;
        }

        private async Task<(ulong?, ulong?)> GetBlockRange(TokenHolderScanJobLog jobLog, string network)
        {
            var client = _ethereumClient.GetClient(network);
            var latestBlock = await client.LatestBlockNumberAsync();

            ulong? startFromBlockNumber;
            ulong? endBlockNumber;
            if (jobLog is not null)
            {
                startFromBlockNumber = jobLog.LastFetchedBlock;

                var blocksRangeToWide = latestBlock - jobLog.LastFetchedBlock > MaxBlocksRange;
                if (blocksRangeToWide)
                {
                    endBlockNumber = startFromBlockNumber + MaxBlocksRange;
                }
                else
                {
                    endBlockNumber = (ulong) latestBlock;
                }
            }
            else
            {
                endBlockNumber = (ulong) latestBlock;
                startFromBlockNumber = endBlockNumber - MaxBlocksRange;
            }

            endBlockNumber -= BlocksRangeSlippage;

            return (startFromBlockNumber, endBlockNumber);
        }
    }
}