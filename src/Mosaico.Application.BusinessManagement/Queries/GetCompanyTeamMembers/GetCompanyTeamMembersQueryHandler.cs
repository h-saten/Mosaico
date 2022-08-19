using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.BusinessManagement.DTOs;
using Mosaico.Domain.BusinessManagement.Abstractions;
using Mosaico.SDK.Identity.Abstractions;
using Mosaico.SDK.Identity.Models;
using Serilog;

namespace Mosaico.Application.BusinessManagement.Queries.GetCompanyTeamMembers
{
    public class GetCompanyTeamMembersQueryHandler : IRequestHandler<GetCompanyTeamMembersQuery, GetCompanyTeamMembersQueryResponse>
    {
        private readonly IBusinessDbContext _context;
        private readonly ILogger _logger;
        private readonly IUserManagementClient _userManagementClient;

        public GetCompanyTeamMembersQueryHandler(IBusinessDbContext context, IUserManagementClient userManagementClient, ILogger logger = null)
        {
            _context = context;
            _logger = logger;
            _userManagementClient = userManagementClient;
        }

        public async Task<GetCompanyTeamMembersQueryResponse> Handle(GetCompanyTeamMembersQuery request, CancellationToken cancellationToken)
        {
            var companyTeamMembersQuery = _context.TeamMembers.Include(t => t.TeamMemberRole)
                .Where(c => c.CompanyId == request.CompanyId);

            var companyTeamMembers = await companyTeamMembersQuery.AsNoTracking().ToListAsync(cancellationToken: cancellationToken);
            var totalCount = await companyTeamMembersQuery.CountAsync(cancellationToken: cancellationToken);
            var dtos = new List<TeamMemberDTO>();
            
            foreach (var teamMember in companyTeamMembers)
            {
                MosaicoUser user = default;
                if (!string.IsNullOrEmpty(teamMember.UserId))
                {
                    user = await _userManagementClient.GetUserAsync(teamMember.UserId, cancellationToken);
                }

                var dto = new TeamMemberDTO
                {
                    FirstName = user?.FirstName ?? user?.Email ?? teamMember.Email,
                    LastName = user?.LastName,
                    CompanyId = teamMember.CompanyId,
                    Email = user?.Email ?? teamMember.Email,
                    Role = teamMember.TeamMemberRole.Key,
                    IsAccepted = teamMember.IsAccepted,
                    UserId = teamMember.UserId,
                    IsExpired = teamMember.ExpiresAt.HasValue && DateTimeOffset.UtcNow > teamMember.ExpiresAt.Value,
                    Id = teamMember.Id
                };
                dtos.Add(dto);
            }

            return new GetCompanyTeamMembersQueryResponse
            {
                Entities = dtos,
                Total = totalCount
            };
        }
    }
}