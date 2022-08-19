using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Base.Abstractions;
using Mosaico.CommandLine.Base;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Tools.CommandLine.Models;
using Newtonsoft.Json;

namespace Mosaico.Tools.CommandLine.Commands
{
    [Command("import-payment-currency")]
    public class ImportPaymentCurrencyCommand : CommandBase
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly IMigrationRunner _migrationRunner;
        private string _fileLocation;

        public ImportPaymentCurrencyCommand(IWalletDbContext walletDbContext, IMigrationRunner migrationRunner)
        {
            _walletDbContext = walletDbContext;
            _migrationRunner = migrationRunner;
            SetOption("-file", "Location of the json file with list of payment currencies", s => _fileLocation = s);
        }

        public override async Task Execute()
        {
            if (string.IsNullOrWhiteSpace(_fileLocation))
            {
                _fileLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "currencies.json");
            }
            
            if (!File.Exists(_fileLocation))
            {
                throw new Exception($"File {_fileLocation} does not exist");
            }
            
            _migrationRunner.RunMigrations();
            
            var file = await File.ReadAllTextAsync(_fileLocation, Encoding.UTF8);
            var paymentCurrencyDTOs = JsonConvert.DeserializeObject<List<PaymentCurrencyImportDTO>>(file);
            foreach (var paymentCurrencyImportDto in paymentCurrencyDTOs)
            {
                var paymentCurrency = await _walletDbContext.PaymentCurrencies.FirstOrDefaultAsync(pc => pc.Name == paymentCurrencyImportDto.Name && pc.Chain == paymentCurrencyImportDto.Network);
                if (paymentCurrency != null)
                {
                    await UpdatePCAsync(paymentCurrency, paymentCurrencyImportDto);
                }
                else
                {
                    await CreatePCAsync(paymentCurrencyImportDto);
                }
            }
            await _walletDbContext.SaveChangesAsync();
        }

        private async Task CreatePCAsync(PaymentCurrencyImportDTO dto)
        {
            var paymentCurrency = new PaymentCurrency
            {
                Name = dto.Name,
                Ticker = dto.Symbol,
                LogoUrl = dto.LogoUrl,
                ContractAddress = dto.Address,
                Chain = dto.Network,
                DecimalPlaces = dto.Decimals,
                NativeChainCurrency = dto.IsNative
            };
            _walletDbContext.PaymentCurrencies.Add(paymentCurrency);
        }

        private async Task UpdatePCAsync(PaymentCurrency currency, PaymentCurrencyImportDTO dto)
        {
            currency.Ticker = dto.Symbol;
            currency.LogoUrl = dto.LogoUrl;
            currency.NativeChainCurrency = dto.IsNative;
            currency.DecimalPlaces = dto.Decimals;
            _walletDbContext.PaymentCurrencies.Update(currency);
            await Task.CompletedTask;
        }
    }
}