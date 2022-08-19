using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.DTOs;
using Mosaico.Application.Wallet.Services;
using Mosaico.Base;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.SDK.Identity.Abstractions;
using Mosaico.SDK.ProjectManagement.Abstractions;
using Serilog;

namespace Mosaico.Application.Wallet.Queries.GetProjectTransactions
{
    public class GetProjectTransactionsQueryHandler : IRequestHandler<GetProjectTransactionsQuery, GetProjectTransactionsQueryResponse>
    {
        private readonly IWalletDbContext _context;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IProjectTransactionsDTOAggregatorService _aggregatorService;
        private readonly IProjectManagementClient _projectManagementClient;
        private readonly IUserManagementClient _userManagementClient;

        public GetProjectTransactionsQueryHandler(
            IWalletDbContext walletDbContext,
            IProjectTransactionsDTOAggregatorService aggregatorService,
            IMapper mapper,
            ILogger logger,
            IProjectManagementClient projectManagementClient,
            IUserManagementClient userManagementClient)
        {
            _context = walletDbContext;
            _logger = logger;
            _projectManagementClient = projectManagementClient;
            _userManagementClient = userManagementClient;
            _mapper = mapper;
            _aggregatorService = aggregatorService;
        }

        public async Task<GetProjectTransactionsQueryResponse> Handle(GetProjectTransactionsQuery request, CancellationToken cancellationToken)
        {
            var project = await _projectManagementClient.GetProjectAsync(request.ProjectId, cancellationToken);
            
            if (project == null)
                throw new ProjectNotFoundException(request.ProjectId.ToString());

            if (!project.TokenId.HasValue || project.TokenId.Value == Guid.Empty
                                          || await _context.Tokens.FirstOrDefaultAsync(t => t.Id == project.TokenId.Value,
                                              cancellationToken) == null)
            {
                return new GetProjectTransactionsQueryResponse();
            }
            var stageIds = project.Stages?.Select(s => s.Id);
            var transactionsQuery = _context
                .Transactions
                .Include(m => m.Status)
                .Include(m => m.Type)
                .Include(m => m.PaymentCurrency)
                .OrderByDescending(t => t.CreatedAt)
                .AsNoTracking()
                .Where(t =>
                    t.TokenId == project.TokenId.Value &&
                    (t.ProjectId == project.Id || stageIds.Contains(t.StageId.Value)) &&
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
            else
            {
                transactionsQuery = transactionsQuery.Where(t => t.Status.Key != Domain.Wallet.Constants.TransactionStatuses.Expired);
            }
            
            if(request.PaymentMethods != null && request.PaymentMethods.Any())
            {
                transactionsQuery = transactionsQuery.Where(t => request.PaymentMethods.Contains(t.PaymentProcessor));
            }

            if (!string.IsNullOrWhiteSpace(request.CorrelationId))
            {
                var id = request.CorrelationId.ToLowerInvariant().Trim();
                transactionsQuery = transactionsQuery.Where(t => t.CorrelationId == id);
            }
            
            var transactions = await transactionsQuery.AsNoTracking().Skip(request.Skip).Take(request.Take).ToListAsync(cancellationToken: cancellationToken);
            if (!transactions.Any()) return new GetProjectTransactionsQueryResponse();
            var totalItems = await transactionsQuery.CountAsync(cancellationToken: cancellationToken);
            var users = await _userManagementClient.GetUsersAsync(transactions.Where(t => t.UserId != null).Select(t => t.UserId).ToList(), cancellationToken);
            
            var dtos = new List<ProjectTransactionDTO>();
            
            foreach (var transaction in transactions)
            {
                var dto = await _aggregatorService.FillInDTOAsync(transaction);
                if (!string.IsNullOrWhiteSpace(transaction.UserId))
                {
                    var user = users.FirstOrDefault(u =>
                        u.Id.ToLowerInvariant() == transaction.UserId.ToLowerInvariant());
                    var userName = $"{user?.FirstName} {user?.LastName}".Trim();
                    if (string.IsNullOrWhiteSpace(userName))
                    {
                        userName = user?.Email;
                    }

                    dto.UserName = userName;
                }

                dtos.Add(dto);
            }

            var result = new GetProjectTransactionsQueryResponse
            {
                Entities = dtos,
                Total = totalItems
            };

            return result;
        }
    }
}