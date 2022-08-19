using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Analytics.Base;
using Mosaico.Application.Statistics.Exceptions;
using Mosaico.Base.Tools;
using Mosaico.Domain.Statistics.Abstractions;
using Mosaico.Domain.Statistics.Entities;
using Mosaico.SDK.ProjectManagement.Abstractions;
using Mosaico.SDK.ProjectManagement.Models;

namespace Mosaico.Application.Statistics.Queries.StatisticsSummary
{
    public class StatisticsSummaryQueryHandler : IRequestHandler<StatisticsSummaryQuery, StatisticsSummaryResponse>
    {
        private readonly ITrafficProvider _statisticsProvider;
        private readonly IProjectManagementClient _projectManagementClient;
        private readonly IDateTimeProvider _dateTime;
        private readonly IStatisticsDbContext _statisticsDbContext;
        
        public StatisticsSummaryQueryHandler(
            ITrafficProvider statisticsProvider, 
            IProjectManagementClient projectManagementClient, 
            IDateTimeProvider dateTime, 
            IStatisticsDbContext statisticsDbContext)
        {
            _statisticsProvider = statisticsProvider;
            _projectManagementClient = projectManagementClient;
            _dateTime = dateTime;
            _statisticsDbContext = statisticsDbContext;
        }
        
        public async Task<StatisticsSummaryResponse> Handle(StatisticsSummaryQuery request, CancellationToken cancellationToken)
        {
            var projectDetails = await _projectManagementClient
                .GetProjectAsync(request.ProjectId, cancellationToken);

            if (projectDetails is null)
            {
                throw new ProjectNotFoundException(request.ProjectId);
            }
            
            var response = new StatisticsSummaryResponse();

            await PrepareVisitsStatisticsAsync(projectDetails, response, cancellationToken);

            if (projectDetails.TokenId is not null)
            {
                await PrepareSaleStatisticsAsync(projectDetails, response, cancellationToken);
            }
            
            response.Followers = await _projectManagementClient.SubscribersAmountAsync(projectDetails.Id, cancellationToken);

            return response;
        }
        
        private async Task PrepareVisitsStatisticsAsync(MosaicoProject project, StatisticsSummaryResponse response, CancellationToken cancellationToken)
        {
            var currentDate = _dateTime.Now();
            var stats = await _statisticsProvider.PagesVisitsCounterAsync(project.Slug, currentDate.AddMonths(-1), currentDate);
            response.TokenPageVisits = stats.TokenPageVisits;
            response.FundPageVisits = stats.BuyPageVisits;
        }

        private async Task PrepareSaleStatisticsAsync(MosaicoProject project, StatisticsSummaryResponse response, CancellationToken cancellationToken)
        {
            var transactions = await _statisticsDbContext
                .PurchaseTransactions
                .AsNoTracking()
                .Where(x => x.TokenId == project.TokenId)
                .ToListAsync(cancellationToken);

            if (transactions.Count == 0)
            {
                return;
            }

            var transactionsCounter = transactions.Count;
            response.TransactionsCounter = transactionsCounter;
            response.MedianTransactionAmount = CalculateTransactionMedianAmount(transactions);

            var investedSum = transactions.Sum(x => x.USDTAmount);
            response.RaisedFunds = investedSum;
            response.AverageTransaction = investedSum / transactionsCounter;
            
            response.SmallestTransactionAmount = transactions.Min(x => x.USDTAmount);
            response.HighestTransactionAmount = transactions.Max(x => x.USDTAmount);
        }
        
        private decimal CalculateTransactionMedianAmount(List<PurchaseTransaction> transactions)
        {
            var transactionsAmount = transactions.Count;
            if (transactionsAmount == 0)
                return 0;
            
            if (transactionsAmount == 1)
                return transactions[0].USDTAmount;
            
            if (transactionsAmount > 1 && transactionsAmount % 2 == 0)
            {
                var firstIndex = transactionsAmount / 2 - 1;
                
                var firstMiddleTransaction = transactions[firstIndex];
                var secondMiddleTransaction = transactions[firstIndex + 1];

                var firstMiddleTransactionCurrencyAmount = firstMiddleTransaction.USDTAmount;
                var secondMiddleTransactionCurrencyAmount = secondMiddleTransaction.USDTAmount;
                
                return (firstMiddleTransactionCurrencyAmount + secondMiddleTransactionCurrencyAmount) / 2;
            }
            
            var middleElementIndex = transactionsAmount / 2;
            var middleTransaction = transactions[middleElementIndex];
            return middleTransaction.USDTAmount;
        }
    }
}