using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.Services.Models;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.CommandLine.Base;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.Tools.CommandLine.Models;
using Newtonsoft.Json;
using Serilog;

namespace Mosaico.Tools.CommandLine.Commands
{
    public class DividendImportDTO
    {
        [CsvHelper.Configuration.Attributes.Index(0)]
        public string Wallet { get; set; }
        
        [CsvHelper.Configuration.Attributes.Index(1)]
        public string PayDividendWith { get; set; }
        
        [CsvHelper.Configuration.Attributes.Index(2)]
        public decimal TokenAmount { get; set; }
        
        [CsvHelper.Configuration.Attributes.Index(3)]
        public decimal DividendAmount { get; set; }

    }
    
    [Command("pay-dividend")]
    public class PayDividendCommand : CommandBase
    {
        private Guid _companyId;
        private bool _isDryRun;
        private string _filePath;
        private readonly IWalletDbContext _dbContext;
        private readonly ICompanyWalletService _companyWalletService;
        private readonly ILogger _logger;

        public PayDividendCommand(IWalletDbContext dbContext, ICompanyWalletService companyWalletService, ILogger logger)
        {
            _dbContext = dbContext;
            _companyWalletService = companyWalletService;
            _logger = logger;
            SetOption("-companyId", "company id", (s) => _companyId = Guid.Parse(s));
            SetOption("-file", "file", (s) => _filePath = s);
            SetOption("-dry", "Is dry run", s => _isDryRun = string.IsNullOrWhiteSpace(s) || bool.Parse(s));
        }
        
        public override async Task Execute()
        {
            if (!File.Exists(_filePath))
            {
                throw new Exception("File not found");
            }

            var companyWallet = await _dbContext.CompanyWallets.FirstOrDefaultAsync(c => c.CompanyId == _companyId);
            if (companyWallet == null)
            {
                throw new CompanyWalletNotFoundException(_companyId.ToString());
            }
            
            _logger.Information($"Company wallet {companyWallet.AccountAddress} was found");
            
            var file = await File.ReadAllBytesAsync(_filePath);
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false
            };
            var records = new List<DividendImportDTO>();
            using (var memoryStream = new MemoryStream(file))
            {
                using (var reader = new StreamReader(memoryStream))
                {
                    using (var csv = new CsvReader(reader, config))
                    {
                        records.AddRange(csv.GetRecords<DividendImportDTO>());
                        _logger.Information($"There were {records.Count} found in CSV file");
                    }
                }
            }

            foreach (var item in records)
            {
                _logger.Information($"Starting transfer of {item.DividendAmount} {item.PayDividendWith} to the wallet {item.Wallet}");
                var isToken = await _dbContext.Tokens.AnyAsync(t => t.Symbol == item.PayDividendWith);
                var isCurrency = await _dbContext.PaymentCurrencies.AnyAsync(t => t.Ticker == item.PayDividendWith);
                if (isToken)
                {
                    var token = await _dbContext.Tokens.FirstOrDefaultAsync(t => t.Symbol == item.PayDividendWith);
                    if (!_isDryRun)
                    {
                        var transaction = await _companyWalletService.TransferTokenAsync(companyWallet, token.Address, item.Wallet, item.DividendAmount);
                        _logger.Information($"Transaction hash: {transaction}");
                    }

                    _logger.Information($"Successfully transferred the reward");
                }
                else if (isCurrency)
                {
                    var currency = await _dbContext.PaymentCurrencies.FirstOrDefaultAsync(t => t.Ticker == item.PayDividendWith);
                    if (!_isDryRun)
                    {
                        var transaction = await _companyWalletService.TransferTokenAsync(companyWallet, currency.ContractAddress,
                            item.Wallet, item.DividendAmount);
                        _logger.Information($"Transaction hash: {transaction}");
                    }

                    _logger.Information($"Successfully transferred the reward");
                }
                else
                {
                    throw new Exception($"Reward token was not found");
                }
            }
            
            _logger?.Information($"All dividends were paid successfully");
            Console.ReadLine();
        }
    }
}