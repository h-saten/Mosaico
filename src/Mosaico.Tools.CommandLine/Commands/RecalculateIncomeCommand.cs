using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.CommandLine.Base;
using Mosaico.Domain.Wallet.Abstractions;
using Serilog;

namespace Mosaico.Tools.CommandLine.Commands
{
    [Command("recalculate-income")]
    public class RecalculateIncomeCommand : CommandBase
    {
        private Guid _projectId;
        private readonly IWalletDbContext _walletDbContext;
        private readonly ILogger _logger;

        public RecalculateIncomeCommand(IWalletDbContext walletDbContext, ILogger logger)
        {
            _walletDbContext = walletDbContext;
            _logger = logger;
            SetOption("--projectId", "Project ID", s => _projectId = Guid.Parse(s));
        }

        public override async Task Execute()
        {
            var confirmedTransactionStatus =
                await _walletDbContext.TransactionStatuses.FirstOrDefaultAsync(t =>
                    t.Key == Domain.Wallet.Constants.TransactionStatuses.Confirmed);
            var transactions = await _walletDbContext.Transactions.Where(t => t.ProjectId == _projectId && t.StatusId == confirmedTransactionStatus.Id)
                .ToListAsync();
            var transactionsWithoutConverted = transactions.Where(t => t.PayedInUSD == null || t.PayedInUSD == 0);
            _logger?.Information($"Total confirmed transactions: {transactions.Count}");
            _logger?.Information($"Total transactions without converted payment rate: {transactionsWithoutConverted.Count()}");
            foreach (var transaction in transactionsWithoutConverted)
            {
                _logger?.Warning($"Transaction - {transaction.CorrelationId} - {transaction.PayedAmount} {transaction.Currency}");
            }
            _logger?.Information($"Total Gathered Capital: ${transactions.Sum(t => t.PayedInUSD)}");
            Console.ReadLine();
        }
    }
}