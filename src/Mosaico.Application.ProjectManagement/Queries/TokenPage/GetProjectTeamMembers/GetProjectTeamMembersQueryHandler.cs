using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.DTOs;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Serilog;

namespace Mosaico.Application.ProjectManagement.Queries.TokenPage.GetProjectTeamMembers
{
    public class GetProjectTeamMembersQueryHandler : IRequestHandler<GetProjectTeamMembersQuery, GetProjectTeamMembersQueryResponse>
    {
        private readonly IProjectDbContext _context;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public GetProjectTeamMembersQueryHandler(IProjectDbContext context, IMapper mapper, ILogger logger = null)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetProjectTeamMembersQueryResponse> Handle(GetProjectTeamMembersQuery request, CancellationToken cancellationToken)
        {
            var projectTeamMembersQuery = _context.PageTeamMembers.AsQueryable()
                .Where(p => p.PageId == request.PageId).OrderBy(x=>x.Order);
            var projectTeamMembers = await projectTeamMembersQuery.AsNoTracking().ToListAsync(cancellationToken: cancellationToken);
            var totalCount = await projectTeamMembersQuery.CountAsync(cancellationToken: cancellationToken);
            var dtos = projectTeamMembers.Select(teamMember => _mapper.Map<ProjectTeamMemberDTO>(teamMember)).ToList();
            return new GetProjectTeamMembersQueryResponse
            {
                Entities = dtos,
                Total = totalCount
            };
        }
    }
}