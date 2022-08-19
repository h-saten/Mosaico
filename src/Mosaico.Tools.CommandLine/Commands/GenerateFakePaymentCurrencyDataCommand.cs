using System;
using System.Threading.Tasks;
using Mosaico.Base.Abstractions;
using Mosaico.CommandLine.Base;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Serilog;
using Constants = Mosaico.Blockchain.Base.Constants;

namespace Mosaico.Tools.CommandLine.Commands
{
    [Command("generate-fake-payment-currency-data", "Generates fake payment currencies data in database")]
    public class GenerateFakePaymentCurrencyDataCommand : CommandBase
    {
        private readonly IProjectDbContext _context;
        private readonly IWalletDbContext _walletContext;
        private readonly IMigrationRunner _migrationRunner;
        private readonly ILogger _logger;
        private readonly ITokenService _tokenService;
        
        public GenerateFakePaymentCurrencyDataCommand(
            IProjectDbContext context, 
            IWalletDbContext walletContext, 
            ILogger logger, 
            IMigrationRunner migrationRunners, 
            ITokenService tokenService)
        {
            _context = context;
            _walletContext = walletContext;
            _logger = logger;
            _migrationRunner = migrationRunners;
            _tokenService = tokenService;
        }

        public override async Task Execute()
        {
            _migrationRunner?.RunMigrations();
            await using var transaction = _context.BeginTransaction();
            var network = Constants.BlockchainNetworks.Default;
            try
            {
                var supplyDecimals = (long) Math.Pow(10, 18);
                var address = await _tokenService.DeployERC20Async(network, t =>
                {
                    t.Name = "Tether";
                    t.Symbol = "USDT";
                    t.IsBurnable = true;
                    t.IsMintable = true;
                    t.InitialSupply = 100000 * supplyDecimals;
                });
                
                var tether = new PaymentCurrency
                {
                    Id = Guid.NewGuid(),
                    Name = "Tether",
                    Ticker = "USDT",
                    LogoUrl = "https://cdn.worldvectorlogo.com/logos/tether-1.svg",
                    ContractAddress = address,
                    Chain = network,
                    DecimalPlaces = 18
                };

                await _walletContext.PaymentCurrencies.AddAsync(tether);
                await _walletContext.SaveChangesAsync();

                await transaction.CommitAsync();
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