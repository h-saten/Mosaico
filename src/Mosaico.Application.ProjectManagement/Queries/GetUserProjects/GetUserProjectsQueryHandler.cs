using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.DTOs;
using Mosaico.Application.ProjectManagement.Services;
using Mosaico.Base;
using Mosaico.Domain.ProjectManagement.Abstractions;

namespace Mosaico.Application.ProjectManagement.Queries.GetUserProjects
{
    public class GetUserProjectsQueryHandler : IRequestHandler<GetUserProjectsQuery, PaginatedResult<ProjectDTO>>
    {
        private readonly IProjectDbContext _projectDbContext;
        private readonly IProjectDTOAggregatorService _aggregatorService;

        public GetUserProjectsQueryHandler(IProjectDbContext projectDbContext, IProjectDTOAggregatorService aggregatorService)
        {
            _projectDbContext = projectDbContext;
            _aggregatorService = aggregatorService;
        }

        public async Task<PaginatedResult<ProjectDTO>> Handle(GetUserProjectsQuery request, CancellationToken cancellationToken)
        {
            var memberShipIds = await _projectDbContext.ProjectMembers.AsNoTracking().Where(p => p.UserId == request.UserId && p.IsAccepted)
                .Select(m => m.ProjectId)
                .ToListAsync(cancellationToken: cancellationToken);

            var projectQueries = _projectDbContext.Projects.Include(p => p.Status)
                .Include(p => p.Stages).ThenInclude(s => s.Status)
                .Include(p => p.Crowdsale)
                .Include(p => p.Likes)
                .Include(p => p.Page).ThenInclude(p => p.ShortDescription).ThenInclude(sh => sh.Translations)
                .Include(p => p.Page).ThenInclude(p => p.PageCovers).ThenInclude(sh => sh.Translations)
                .Where(p => memberShipIds.Contains(p.Id))
                .OrderByDescending(p => p.CreatedAt)
                .AsQueryable();
            
            var projects = await projectQueries.AsNoTracking().Skip(request.Skip).Take(request.Take).ToListAsync(cancellationToken: cancellationToken);
            var totalCount = await projectQueries.CountAsync(cancellationToken: cancellationToken);
            
            var dtos = new List<ProjectDTO>();
            foreach (var project in projects)
            {
                var dto = await _aggregatorService.FillInDTOAsync(project);
                dtos.Add(dto);
            }

            return new PaginatedResult<ProjectDTO>
            {
                Entities = dtos,
                Total = totalCount
            };
        }
    }
}