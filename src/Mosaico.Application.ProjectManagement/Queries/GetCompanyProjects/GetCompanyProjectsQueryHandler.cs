using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.DTOs;
using Mosaico.Application.ProjectManagement.Services;
using Mosaico.Authorization.Base;
using Mosaico.Base;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities;
using Mosaico.SDK.Identity.Abstractions;
using Mosaico.SDK.Identity.Models;
using Mosaico.SDK.Wallet.Abstractions;

namespace Mosaico.Application.ProjectManagement.Queries.GetCompanyProjects
{
    public class GetCompanyProjectsQueryHandler : IRequestHandler<GetCompanyProjectsQuery, PaginatedResult<ProjectDTO>>
    {
        private readonly IProjectDbContext _projectDbContext;
        private readonly IProjectDTOAggregatorService _aggregator;
        private readonly IUserManagementClient _managementClient;
        private readonly ICurrentUserContext _userContext;
        
        public GetCompanyProjectsQueryHandler(IProjectDbContext projectDbContext, IProjectDTOAggregatorService aggregator, IUserManagementClient managementClient, ICurrentUserContext userContext)
        {
            _projectDbContext = projectDbContext;
            _aggregator = aggregator;
            _managementClient = managementClient;
            _userContext = userContext;
        }

        public async Task<PaginatedResult<ProjectDTO>> Handle(GetCompanyProjectsQuery request, CancellationToken cancellationToken)
        {
            var projectsQuery = _projectDbContext.Projects.Include(p => p.Status)
                .Include(p => p.Stages).ThenInclude(s => s.Status)
                .Include(p => p.Crowdsale)
                .Include(p => p.Likes)
                .Include(p => p.Page)
                .ThenInclude(p => p.ShortDescription).ThenInclude(sh => sh.Translations)
                .Include(p => p.Page).ThenInclude(p => p.PageCovers).ThenInclude(s => s.Translations)
                .Where(p => p.CompanyId == request.CompanyId)
                .OrderByDescending(p => p.CreatedAt)
                .AsQueryable();

            var permissions = new List<MosaicoPermission>();
            if (_userContext.IsAuthenticated)
            {
                permissions = await _managementClient.GetUserPermissionsAsync(_userContext.UserId, request.CompanyId, cancellationToken);
            }

            if (!permissions.Any(p => p.Key == Authorization.Base.Constants.Permissions.Company.CanRead) && !_userContext.IsGlobalAdmin)
            {
                projectsQuery = projectsQuery.Where(p =>
                    p.Status.Key != Domain.ProjectManagement.Constants.ProjectStatuses.New
                    && p.Status.Key != Domain.ProjectManagement.Constants.ProjectStatuses.Declined
                    && p.Status.Key != Domain.ProjectManagement.Constants.ProjectStatuses.UnderReview && p.IsVisible);
            }

            var projects = await projectsQuery.Skip(request.Skip).Take(request.Take).ToListAsync(cancellationToken);
            var totalCount = await projectsQuery.CountAsync(cancellationToken);
            
            var dtos = new List<ProjectDTO>();
            foreach (var project in projects)
            {
                var dto = await _aggregator.FillInDTOAsync(project);
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