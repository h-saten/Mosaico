using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.DTOs;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Serilog;

namespace Mosaico.Application.ProjectManagement.Queries.GetProjectMembers
{
    public class GetProjectMembersQueryHandler : IRequestHandler<GetProjectMembersQuery, GetProjectMembersQueryResponse>
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IProjectDbContext _projectDbContext;

        public GetProjectMembersQueryHandler(IMapper mapper, IProjectDbContext projectDbContext, ILogger logger = null)
        {
            _mapper = mapper;
            _logger = logger;
            _projectDbContext = projectDbContext;
        }

        public async Task<GetProjectMembersQueryResponse> Handle(GetProjectMembersQuery request, CancellationToken cancellationToken)
        {
            var project = await _projectDbContext.Projects.Include(p => p.Members).ThenInclude(m => m.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == request.ProjectId, cancellationToken);
            if (project == null)
            {
                throw new ProjectNotFoundException(request.ProjectId);
            }

            return new GetProjectMembersQueryResponse
            {
                Entities = project.Members.Select(m => _mapper.Map<ProjectMemberDTO>(m)).ToList()
            };
        }
    }
}