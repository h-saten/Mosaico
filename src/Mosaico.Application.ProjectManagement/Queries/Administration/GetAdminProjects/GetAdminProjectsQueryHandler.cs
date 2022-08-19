using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.DTOs;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Serilog;

namespace Mosaico.Application.ProjectManagement.Queries.Administration.GetAdminProjects
{
    public class GetAdminProjectsQueryHandler : IRequestHandler<GetAdminProjectsQuery, GetAdminProjectsQueryResponse>
    {
        private readonly IProjectDbContext _projectDbContext;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public GetAdminProjectsQueryHandler(IProjectDbContext projectDbContext, ILogger logger, IMapper mapper)
        {
            _projectDbContext = projectDbContext;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<GetAdminProjectsQueryResponse> Handle(GetAdminProjectsQuery request, CancellationToken cancellationToken)
        {
            var projectsQuery = _projectDbContext.Projects.Include(p => p.Status)
                .Include(p => p.Crowdsale)
                .OrderByDescending(p => p.CreatedAt)
                .AsQueryable();
            
            if (!string.IsNullOrWhiteSpace(request.FreeTextSearch))
            {
                projectsQuery = projectsQuery.Where(t =>
                    t.Title.Contains(request.FreeTextSearch) || t.Slug.Contains(request.FreeTextSearch));
            }

            if (!string.IsNullOrWhiteSpace(request.Status))
            {
                projectsQuery = projectsQuery.Where(t => t.Status.Key == request.Status);
            }
            
            var projects = await projectsQuery.AsNoTracking().Skip(request.Skip).Take(request.Take).ToListAsync(cancellationToken: cancellationToken);
            var totalCount = await projectsQuery.CountAsync(cancellationToken: cancellationToken);
            var dtos = projects.Select(project => _mapper.Map<ProjectDTO>(project));
            
            return new GetAdminProjectsQueryResponse
            {
                Entities = dtos,
                Total = totalCount
            };
        }
    }
}