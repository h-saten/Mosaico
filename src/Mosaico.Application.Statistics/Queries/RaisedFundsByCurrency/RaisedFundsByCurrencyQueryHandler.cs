using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Statistics.Exceptions;
using Mosaico.Domain.Statistics.Abstractions;
using Mosaico.SDK.ProjectManagement.Abstractions;

namespace Mosaico.Application.Statistics.Queries.RaisedFundsByCurrency
{
    public class RaisedFundsByCurrencyQueryHandler : IRequestHandler<RaisedFundsByCurrencyQuery, RaisedFundsByCurrencyResponse>
    {
        private readonly IStatisticsDbContext _statisticsDbContext;
        private readonly IProjectManagementClient _projectManagementClient;
        
        public RaisedFundsByCurrencyQueryHandler(
            IStatisticsDbContext statisticsDbContext, 
            IProjectManagementClient projectManagementClient)
        {
            _statisticsDbContext = statisticsDbContext;
            _projectManagementClient = projectManagementClient;
        }
        
        public async Task<RaisedFundsByCurrencyResponse> Handle(RaisedFundsByCurrencyQuery request, CancellationToken cancellationToken)
        {
            var projectDetails = await _projectManagementClient
                .GetProjectAsync(request.ProjectId, cancellationToken);
            
            if (projectDetails is null)
            {
                throw new ProjectNotFoundException(request.ProjectId);
            }
            
            var response = new RaisedFundsByCurrencyResponse 
            {
                Statistics = new List<RaisedFundsByCurrencyDto>()
            };

            if (projectDetails.TokenId is null)
            {
                return response;
            }

            var raisedCapitalCurrencyDistinct = await _statisticsDbContext
                .PurchaseTransactions
                .AsNoTracking()
                .Where(m => m.TokenId == projectDetails.TokenId)
                .GroupBy(x => x.Currency, (currency, currencyTransactions) => new RaisedFundsByCurrencyDto
                {
                    Currency = currency,
                    Amount = currencyTransactions.Sum(x => x.Payed),
                    UsdtAmount = currencyTransactions.Sum(x => x.USDTAmount)
                })
                .ToListAsync(cancellationToken);

            response.Statistics = raisedCapitalCurrencyDistinct;
            return response;
        }
    }
}