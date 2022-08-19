using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Statistics.Exceptions;
using Mosaico.Base.Tools;
using Mosaico.Domain.Statistics.Abstractions;
using Mosaico.SDK.ProjectManagement.Abstractions;

namespace Mosaico.Application.Statistics.Queries.DailyRaisedCapital
{
    public class DailyRaisedCapitalQueryHandler : IRequestHandler<DailyRaisedCapitalQuery, DailyRaisedCapitalResponse>
    {
        private readonly IStatisticsDbContext _statisticsDbContext;
        private readonly IProjectManagementClient _projectManagementClient;
        private readonly IDateTimeProvider _dateTimeProvider;
        
        public DailyRaisedCapitalQueryHandler(
            IStatisticsDbContext statisticsDbContext, 
            IProjectManagementClient projectManagementClient, 
            IDateTimeProvider dateTimeProvider)
        {
            _statisticsDbContext = statisticsDbContext;
            _projectManagementClient = projectManagementClient;
            _dateTimeProvider = dateTimeProvider;
        }
        
        public async Task<DailyRaisedCapitalResponse> Handle(DailyRaisedCapitalQuery request, CancellationToken cancellationToken)
        {
            var projectDetails = await _projectManagementClient
                .GetProjectAsync(request.ProjectId, cancellationToken);

            if (projectDetails is null)
            {
                throw new ProjectNotFoundException(request.ProjectId);
            }

            var response = new DailyRaisedCapitalResponse
            {
                Statistics = new List<RaisedCapitalDto>()
            };
            
            if (projectDetails.TokenId is null)
            {
                return response;
            }
            
            var selectedMonth = _dateTimeProvider.Now().AddMonths(-request.FromMonthAgo);
            
            var lastDayOfMonth = DateTime.UtcNow.Day;

            if (request.FromMonthAgo != 0)
            {
                lastDayOfMonth = DateTime.DaysInMonth(selectedMonth.Year, selectedMonth.Month);
            }
            
            var firstMonthDayDate = new DateTime(selectedMonth.Year, selectedMonth.Month, 1, 00, 00, 00);
            var lastMonthDayDate = new DateTime(selectedMonth.Year, selectedMonth.Month, lastDayOfMonth, 23, 59, 59);

            var transactions = await _statisticsDbContext
                .PurchaseTransactions
                .AsNoTracking()
                .Where(m => m.Date >= firstMonthDayDate 
                            && m.Date < lastMonthDayDate 
                            && m.TokenId == projectDetails.TokenId)
                .Select(m => new
                {
                    UsdtAmount = m.USDTAmount,
                    m.Currency,
                    m.Date
                })
                .ToListAsync(cancellationToken);
            
            var result = transactions
                .GroupBy(m => m.Date.Date, (key, data) => new RaisedCapitalDto
                {
                    UsdtAmount = Math.Round(data.Sum(x => x.UsdtAmount)),
                    Date = key.ToString("yyyy-MM-dd")
                })
                .ToList();

            FillDayGaps(lastDayOfMonth, selectedMonth, result);

            response.Statistics = result.OrderBy(m => m.Date).ToList();

            return response;
        }

        private void FillDayGaps(int lastDayOfMonth, DateTimeOffset selectedMonth, List<RaisedCapitalDto> result)
        {
            for (int i = 1; i <= lastDayOfMonth; i++)
            {
                var date = new DateTime(selectedMonth.Year, selectedMonth.Month, i).ToString("yyyy-MM-dd");
                var dayStatisticsExist =
                    result.Any(m => m.Date == date);

                if (!dayStatisticsExist)
                {
                    result.Add(new RaisedCapitalDto
                    {
                        UsdtAmount = 0,
                        Date = date
                    });
                }
            }
        }
    }
}