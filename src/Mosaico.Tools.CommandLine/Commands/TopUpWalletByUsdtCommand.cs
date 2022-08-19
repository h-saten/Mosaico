using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Base.Abstractions;
using Mosaico.CommandLine.Base;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.Integration.Blockchain.Ethereum.MosaicoERC20v1;
using Serilog;

namespace Mosaico.Tools.CommandLine.Commands
{
    [Command("usdt-wallet-topup", "Top up wallet with usdt")]
    public class TopUpWalletByUsdtCommand : CommandBase
    {
        private readonly IProjectDbContext _context;
        private readonly IWalletDbContext _walletContext;
        private readonly IMigrationRunner _migrationRunner;
        private readonly IIdentityContext _identityContext;
        private readonly IEthereumClientFactory _ethereumClientFactory;
        private readonly ILogger _logger;

        public TopUpWalletByUsdtCommand(
            IProjectDbContext context, 
            IWalletDbContext walletContext, 
            ILogger logger, 
            IMigrationRunner migrationRunners, 
            IIdentityContext identityContext, IEthereumClientFactory ethereumClientFactory)
        {
            _context = context;
            _walletContext = walletContext;
            _logger = logger;
            _migrationRunner = migrationRunners;
            _identityContext = identityContext;
            _ethereumClientFactory = ethereumClientFactory;
        }

        public override async Task Execute()
        {
            _migrationRunner?.RunMigrations();
            await using var transaction = _context.BeginTransaction();
            try
            {
                var tetherEntity = await _walletContext
                    .PaymentCurrencies
                    .AsNoTracking()
                    .Where(x => x.Ticker == "USDT" && x.Chain == "Ethereum")
                    .SingleOrDefaultAsync();

                if (tetherEntity is null)
                {
                    return;
                }
                
                var usdcEntity = await _walletContext
                    .PaymentCurrencies
                    .AsNoTracking()
                    .Where(x => x.Ticker == "USDC" && x.Chain == "Ethereum")
                    .SingleOrDefaultAsync();

                if (usdcEntity is null)
                {
                    return;
                }

                var userEmailsToTopUp = new List<string>() {"dev@mosaico.ai"};

                var users = await _identityContext
                    .Users
                    .AsNoTracking()
                    .Where(x => userEmailsToTopUp.Contains(x.Email))
                    .ToListAsync();

                var usersAmount = users.Count;
                if (usersAmount > 0)
                {
                    var userIds = users.Select(x => x.Id).ToList();
                    _logger.Information($"Amount of users to top up {usersAmount}");
                    
                    var walletsToTopUp = await _walletContext
                        .Wallets
                        .AsNoTracking()
                        .Where(x => userIds.Contains(x.UserId))
                        .ToListAsync();
                    
                    var tokensPerUser = BigInteger.Multiply(300, (ulong) Math.Pow(10, 6));
                    _logger.Information($"Tokens per user {tokensPerUser}");

                    var usdtAddress = tetherEntity.ContractAddress;
                    var usdcAddress = usdcEntity.ContractAddress;
                    _logger.Information($"Tether contract address: '{usdtAddress}'");

                    var counter = 1;
                    foreach (var wallet in walletsToTopUp)
                    {
                        var client = _ethereumClientFactory.GetServiceFactory(wallet.Network);
                        var usdtContract = await client.GetServiceAsync<MosaicoERC20v1Service>(usdtAddress, string.Empty);
                        var usdtSupply = await usdtContract.TotalSupplyQueryAsync();
                        _logger.Information($"USDT supply: {usdtSupply}");
                        await usdtContract.TransferRequestAndWaitForReceiptAsync(wallet.AccountAddress, tokensPerUser);
                        _logger.Information($"Transfer: {counter} of {usersAmount}. Transfer of {tokensPerUser.ToString()} USDT to wallet {wallet.AccountAddress}");
                        var result = await usdtContract.BalanceOfQueryAsync(wallet.AccountAddress);
                        _logger.Information($"Transfer: {counter} of {usersAmount}. Wallet {wallet.AccountAddress} balance: {result}");
                        counter++;
                    }

                    counter = 1;
                    foreach (var wallet in walletsToTopUp)
                    {
                        var client = _ethereumClientFactory.GetServiceFactory(wallet.Network);
                        var usdcContract = await client.GetServiceAsync<MosaicoERC20v1Service>(usdcAddress, string.Empty);
                        var usdtSupply = await usdcContract.TotalSupplyQueryAsync();
                        _logger.Information($"USDC supply: {usdtSupply}");
                        await usdcContract.TransferRequestAndWaitForReceiptAsync(wallet.AccountAddress, tokensPerUser);
                        _logger.Information($"Transfer: {counter} of {usersAmount}. Transfer of {tokensPerUser.ToString()} USDC to wallet {wallet.AccountAddress}");
                        var result = await usdcContract.BalanceOfQueryAsync(wallet.AccountAddress);
                        _logger.Information($"Transfer: {counter} of {usersAmount}. Wallet {wallet.AccountAddress} balance: {result}");
                        counter++;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.Error(ex, "");
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}