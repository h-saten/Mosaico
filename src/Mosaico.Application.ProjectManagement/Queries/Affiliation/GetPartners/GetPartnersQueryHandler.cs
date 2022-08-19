using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.DTOs.Affiliation;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.SDK.Identity.Abstractions;
using Serilog;

namespace Mosaico.Application.ProjectManagement.Queries.Affiliation.GetPartners
{
    public class GetPartnersQueryHandler : IRequestHandler<GetPartnersQuery, GetPartnersQueryResponse>
    {
        private readonly IProjectDbContext _projectDbContext;
        private readonly IUserManagementClient _managementClient;
        private readonly ILogger _logger;
        
        public GetPartnersQueryHandler(IProjectDbContext projectDbContext, IUserManagementClient managementClient, ILogger logger)
        {
            _projectDbContext = projectDbContext;
            _managementClient = managementClient;
            _logger = logger;
        }

        public async Task<GetPartnersQueryResponse> Handle(GetPartnersQuery request, CancellationToken cancellationToken)
        {
            var affiliation = await _projectDbContext.ProjectAffiliations.FirstOrDefaultAsync(a => a.ProjectId == request.ProjectId, cancellationToken);
            if(affiliation == null) return new GetPartnersQueryResponse();
            
            var partnersQuery = _projectDbContext.Partners.OrderByDescending(p => p.CreatedAt).Where(p => p.ProjectAffiliationId == affiliation.Id);
            var count = await partnersQuery.CountAsync(cancellationToken);
            var items = await partnersQuery.Skip(request.Skip).Take(request.Take).ToListAsync(cancellationToken);
            
            var dtos = new List<PartnerDTO>();
            foreach (var partner in items)
            {
                if (partner.UserAffiliation == null)
                {
                    _logger?.Warning($"Partner {partner.Id} does not have any user affiliation assigned");
                    continue;
                }

                var transactionsQuery = _projectDbContext.PartnerTransactions.Where(p => p.PartnerId == partner.Id);
                var user = await _managementClient.GetUserAsync(partner.UserAffiliation.UserId, cancellationToken);
                var dto = new PartnerDTO
                {
                    Id = partner.Id,
                    CreatedAt = partner.CreatedAt,
                    Email = user.Email,
                    Name = $"{user.FirstName} {user.LastName}".Trim(),
                    Status = partner.Status,
                    PaymentStatus = partner.PaymentStatus,
                    FailureReason = partner.FailureReason,
                    RewardPercentage = partner.RewardPercentage,
                    TransactionsCount = await transactionsQuery.CountAsync(cancellationToken),
                    EstimatedReward = await transactionsQuery.SumAsync(t => t.EstimatedReward, cancellationToken)
                };
                dtos.Add(dto);
            }

            return new GetPartnersQueryResponse
            {
                Entities = dtos,
                Total = count
            };
        }
    }
}