using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Export;
using Mosaico.Base.Tools;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Export.Base;
using Mosaico.SDK.Identity.Abstractions;
using Mosaico.SDK.ProjectManagement.Abstractions;
using Serilog;

namespace Mosaico.Application.Wallet.Commands.Transactions.ExportTransactions
{
    public class ExportTransactionsCommandHandler : IRequestHandler<ExportTransactionsCommand, ExportTransactionsCommandResponse>
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly IExporter<CsvTransaction> _transactionExporter;
        private readonly IUserManagementClient _managementClient;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ILogger _logger;
        private readonly IProjectManagementClient _projectManagementClient;

        public ExportTransactionsCommandHandler(IWalletDbContext walletDbContext, IExporter<CsvTransaction> transactionExporter, IUserManagementClient managementClient, IDateTimeProvider dateTimeProvider, IProjectManagementClient projectManagementClient, ILogger logger)
        {
            _walletDbContext = walletDbContext;
            _transactionExporter = transactionExporter;
            _managementClient = managementClient;
            _dateTimeProvider = dateTimeProvider;
            _projectManagementClient = projectManagementClient;
            _logger = logger;
        }

        public async Task<ExportTransactionsCommandResponse> Handle(ExportTransactionsCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectManagementClient.GetProjectAsync(request.ProjectId, cancellationToken);
            if (project == null)
                throw new ProjectNotFoundException(request.ProjectId.ToString());
            
            _logger?.Information($"Exporting transactions for project {project.Title} for period {request.From} - {request.To}");
            if (!project.TokenId.HasValue || project.TokenId.Value == Guid.Empty
                                          || await _walletDbContext.Tokens.FirstOrDefaultAsync(t => t.Id == project.TokenId.Value,
                                              cancellationToken) == null)
            {
                return null;
            }
            
            var transactionsQuery = _walletDbContext
                .Transactions
                .Include(m => m.Status)
                .Include(m => m.Type)
                .Include(m => m.PaymentCurrency)
                .Include(t => t.SalesAgent)
                .OrderByDescending(t => t.CreatedAt)
                .AsNoTracking()
                .Where(t =>
                    t.ProjectId == project.Id &&
                    t.Type.Key == Domain.Wallet.Constants.TransactionType.Purchase);
            
            if (request.From.HasValue)
            {
                transactionsQuery = transactionsQuery.Where(t => t.CreatedAt >= request.From);
            }

            if (request.To.HasValue)
            {
                transactionsQuery = transactionsQuery.Where(t => t.CreatedAt < request.To);
            }

            if (request.Statuses != null && request.Statuses.Any())
            {
                transactionsQuery = transactionsQuery.Where(t => request.Statuses.Contains(t.Status.Key));
            }
            
            if(request.PaymentMethods != null && request.PaymentMethods.Any())
            {
                transactionsQuery = transactionsQuery.Where(t => request.PaymentMethods.Contains(t.PaymentProcessor));
            }

            var transactions = await transactionsQuery.ToListAsync(cancellationToken);
            _logger?.Information($"Exporting {transactions.Count} transactions for project {project.Title} for period {request.From} - {request.To}");
            if (!transactions.Any())
                return new ExportTransactionsCommandResponse
                {
                    Count = 0,
                    File = null,
                    Filename = $"{project.Title}_{_dateTimeProvider.Now():yyyy-MM-dd}.csv",
                    ContentType = "text/csv"
                };
            var users = await _managementClient.GetUsersAsync(transactions.Where(t => t.UserId != null).Select(t => t.UserId).ToList(), cancellationToken);
            var csv = await _transactionExporter.ExportAsync(transactions.Select(t =>
            {
                var csv = new CsvTransaction
                {
                    Currency = t.Currency,
                    Payed = t.PayedAmount ?? 0,
                    FinishedAt = t.FinishedAt.HasValue ? t.FinishedAt.Value.ToString("g") : "N/A",
                    CorrelationId = t.CorrelationId,
                    PaymentMethod = t.PaymentMethod,
                    PaymentProcessor = t.PaymentProcessor,
                    RefCode = t.RefCode,
                    TokenAmount = t.TokenAmount ?? 0,
                    TransactionHash = t.TransactionHash,
                    PayedInUSD = t.PayedInUSD ?? 0,
                    Status = t.Status.Key,
                    FeePercentage = t.FeePercentage,
                    PaymentProcessorFee = t.Fee,
                    PaymentProcessorFeeInUSD = t.FeeInUSD ?? 0,
                    MosaicoFee = t.MosaicoFee ?? 0,
                    TokenPrice = t.TokenPrice ?? 0,
                    MosaicoFeeInUSD = t.MosaicoFeeInUSD ?? 0,
                    Salesman = t.SalesAgent?.Name,
                    ExchangeRate = t.ExchangeRate ?? 0
                };
                var user = users.FirstOrDefault(u => u.Id.ToLowerInvariant() == t.UserId.ToLowerInvariant());
                csv.User = $"{user?.FirstName} {user?.LastName}";
                csv.UserEmail = user?.Email;
                return csv;
            }).ToList());
            return new ExportTransactionsCommandResponse
            {
                Count = transactions.Count,
                File = csv,
                Filename = $"{project.Title}_{_dateTimeProvider.Now():yyyy-MM-dd}.csv",
                ContentType = "text/csv"
            };
        }
    }
}