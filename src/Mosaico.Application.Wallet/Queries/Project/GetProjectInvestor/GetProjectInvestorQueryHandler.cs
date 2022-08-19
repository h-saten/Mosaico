using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.DTOs;
using Mosaico.Application.Wallet.DTO;
using Mosaico.Application.Wallet.Queries.GetProjectTransactions;
using Mosaico.Application.Wallet.Services;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.SDK.Identity.Abstractions;
using Mosaico.SDK.ProjectManagement.Abstractions;

namespace Mosaico.Application.Wallet.Queries.Project.GetProjectInvestor
{
    public class GetProjectInvestorQueryHandler : IRequestHandler<GetProjectInvestorQuery, GetProjectInvestorQueryResponse>
    {
        private readonly IUserManagementClient _managementClient;
        private readonly IProjectManagementClient _projectManagementClient;
        private readonly IWalletDbContext _walletDbContext;
        private readonly IProjectTransactionsDTOAggregatorService _aggregatorService;

        public GetProjectInvestorQueryHandler(IUserManagementClient managementClient, IProjectManagementClient projectManagementClient, IWalletDbContext walletDbContext, IProjectTransactionsDTOAggregatorService aggregatorService)
        {
            _managementClient = managementClient;
            _projectManagementClient = projectManagementClient;
            _walletDbContext = walletDbContext;
            _aggregatorService = aggregatorService;
        }

        public async Task<GetProjectInvestorQueryResponse> Handle(GetProjectInvestorQuery request, CancellationToken cancellationToken)
        {
            var project = await _projectManagementClient.GetProjectAsync(request.ProjectId, cancellationToken);
            
            if (project == null)
                throw new ProjectNotFoundException(request.ProjectId.ToString());

            if (!project.TokenId.HasValue || project.TokenId.Value == Guid.Empty
                                          || await _walletDbContext.Tokens.FirstOrDefaultAsync(t => t.Id == project.TokenId.Value,
                                              cancellationToken) == null)
            {
                return new GetProjectInvestorQueryResponse();

            }

            var stageIds = project.Stages?.Select(s => s.Id);

            var user = await _managementClient.GetUserAsync(request.UserId, cancellationToken);
            
            var transactions = await _walletDbContext
                .Transactions
                .Include(m => m.Status)
                .Include(m => m.Type)
                .Include(m => m.PaymentCurrency)
                .OrderByDescending(t => t.FinishedAt)
                .AsNoTracking()
                .Where(t =>
                    t.TokenId == project.TokenId.Value &&
                    t.UserId == request.UserId &&
                    t.StageId != null && stageIds.Contains(t.StageId.Value) &&
                    t.Type.Key == Domain.Wallet.Constants.TransactionType.Purchase)
                .ToListAsync(cancellationToken);
            var transactionDtos = new List<ProjectTransactionDTO>();
            
            foreach (var transaction in transactions)
            {
                var dto = await _aggregatorService.FillInDTOAsync(transaction);
                dto.UserName = user.Email;
                transactionDtos.Add(dto);
            }
            
            return new GetProjectInvestorQueryResponse
            {
                User = new ProjectInvestorDTO
                {
                    Email = user.Email,
                    FullName = $"{user.FirstName} {user.LastName}",
                    PhoneNumber = user.PhoneNumber,
                    UserId = request.UserId
                },
                TotalInvestment = transactions.Where(t => t.TokenAmount.HasValue && t.Status.Key == Domain.Wallet.Constants.TransactionStatuses.Confirmed)
                    .Sum(t => t.TokenAmount.Value),
                TotalPayedInUSD = transactions.Where(t => t.PayedInUSD.HasValue && t.Status.Key == Domain.Wallet.Constants.TransactionStatuses.Confirmed)
                    .Sum(t => t.PayedInUSD.Value),
                Transactions = transactionDtos
            };
        }
    }
}