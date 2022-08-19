using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.CommandLine.Base;
using Mosaico.Domain.Wallet.Abstractions;
using Serilog;

namespace Mosaico.Tools.CommandLine.Commands
{
    [Command("recalculate-fee")]
    public class RecalculateFeeCommand : CommandBase
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly IFeeService _feeService;
        private readonly IExchangeRateService _exchangeRateService;
        private Guid _projectId;
        private readonly ILogger _logger;

        public RecalculateFeeCommand(IWalletDbContext walletDbContext, IFeeService feeService, ILogger logger, IExchangeRateService exchangeRateService)
        {
            _walletDbContext = walletDbContext;
            _feeService = feeService;
            _logger = logger;
            _exchangeRateService = exchangeRateService;
            SetOption("-projectId", "project id", (s) => _projectId = Guid.Parse(s));
        }

        public override async Task Execute()
        {
            var transactions = await _walletDbContext.Transactions.Where(t => t.ProjectId != null && t.ProjectId == _projectId &&
                                                                              t.Status.Key == Domain.Wallet.Constants
                                                                                  .TransactionStatuses.Confirmed
                                                                              && t.Type.Key == Domain.Wallet.Constants
                                                                                  .TransactionType.Purchase).ToListAsync();
            
            _logger?.Information($"Found {transactions.Count} to recalculate");
            using (var t = _walletDbContext.BeginTransaction())
            {
                try
                {
                    foreach (var transaction in transactions)
                    {
                        if (transaction.PayedAmount > 0)
                        {
                            var feePercentage = await _feeService.GetFeeAsync(transaction.ProjectId.Value, transaction.StageId);
                            var rate = await _exchangeRateService.GetExchangeRateAsync(transaction.Currency);
                    
                            transaction.FeePercentage = feePercentage;
                            transaction.FeeInUSD = transaction.Fee * rate.Rate;
                            transaction.MosaicoFee = _feeService.GetMosaicoFeeValueAsync(transaction.FeePercentage, transaction.Fee, transaction.PayedAmount.Value);
                            transaction.MosaicoFeeInUSD = transaction.MosaicoFee * rate.Rate;
                            _logger?.Information($"Saving transaction {transaction.CorrelationId} with updated fee");
                            _walletDbContext.Transactions.Update(transaction);
                        }
                    }
                    await _walletDbContext.SaveChangesAsync();
                    _logger?.Information($"Confirm commitment");
                    Console.ReadLine();
                    await t.CommitAsync();
                }
                catch (Exception e)
                {
                    _logger?.Error(e, "Error during transaction fee update");
                    await t.RollbackAsync();
                    throw;
                }
            }
            
        }
    }
}