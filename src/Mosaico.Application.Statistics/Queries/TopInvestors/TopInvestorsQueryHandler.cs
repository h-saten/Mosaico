using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Statistics.Exceptions;
using Mosaico.Domain.Statistics.Abstractions;
using Mosaico.SDK.Identity.Abstractions;
using Mosaico.SDK.ProjectManagement.Abstractions;

namespace Mosaico.Application.Statistics.Queries.TopInvestors
{
    public class TopInvestorsQueryHandler : IRequestHandler<TopInvestorsQuery, TopInvestorsResponse>
    {
        private readonly IStatisticsDbContext _statisticsDbContext;
        private readonly IUserManagementClient _userManagementClient;
        private readonly IProjectManagementClient _projectManagementClient;
        
        public TopInvestorsQueryHandler(IStatisticsDbContext statisticsDbContext, IUserManagementClient userManagementClient, IProjectManagementClient projectManagementClient)
        {
            _statisticsDbContext = statisticsDbContext;
            _userManagementClient = userManagementClient;
            _projectManagementClient = projectManagementClient;
        }
        
        public async Task<TopInvestorsResponse> Handle(TopInvestorsQuery request, CancellationToken cancellationToken)
        {
            var projectDetails = await _projectManagementClient
                .GetProjectAsync(request.ProjectId, cancellationToken);
            
            if (projectDetails is null)
            {
                throw new ProjectNotFoundException(request.ProjectId);
            }

            var response = new TopInvestorsResponse
            {
                Investors = new List<TopInvestorDto>()
            };
            
            if (projectDetails.TokenId is null)
            {
                return response;
            }
            
            var topInvestors = await _statisticsDbContext
                .PurchaseTransactions
                .AsNoTracking()
                .Where(x => x.TokenId == projectDetails.TokenId)
                .GroupBy(t => t.UserId, (userId, transactions) => new
                {
                    UserId = userId,
                    InvestmentsSum = transactions.Sum(t => t.USDTAmount)
                })
                .OrderByDescending(x => x.InvestmentsSum)
                .Take(5)
                .ToListAsync(cancellationToken);
            
            var topInvestorsId = topInvestors.Select(x => x.UserId.ToString()).ToList();

            if (topInvestorsId.Count == 0)
            {
                return response;
            }

            var usersAccount = await _userManagementClient.GetUsersAsync(topInvestorsId, cancellationToken);

            var responseList = new List<TopInvestorDto>();
            foreach (var topInvestor in topInvestors)
            {
                var account = usersAccount.SingleOrDefault(x => x.Id == topInvestor.UserId.ToString());
                responseList.Add(new TopInvestorDto
                {
                    Name = $"{account?.FirstName} {account?.LastName}".TrimEnd(),
                    InvestedAmount = topInvestor.InvestmentsSum
                });
            }
            response.Investors = responseList;
            
            return response;
        }
    }
}