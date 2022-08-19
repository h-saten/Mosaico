using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.DTOs;
using Mosaico.Application.ProjectManagement.Services;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities;
using Mosaico.SDK.Wallet.Abstractions;
using Serilog;

namespace Mosaico.Application.ProjectManagement.Queries.GetProjects
{
    public class GetProjectsQueryHandler : IRequestHandler<GetProjectsQuery, GetProjectsQueryResponse>
    {
        private readonly IProjectDbContext _context;
        private readonly ILogger _logger;
        private readonly IProjectDTOAggregatorService _aggregator;
        
        public GetProjectsQueryHandler(IProjectDbContext context, IProjectDTOAggregatorService aggregator, ILogger logger = null)
        {
            _context = context;
            _aggregator = aggregator;
            _logger = logger;
        }

        public async Task<GetProjectsQueryResponse> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
        {
            var projectsQuery = _context.Projects.Include(p => p.Status)
                    .Include(p => p.Stages).ThenInclude(s => s.Status)
                    .Include(p => p.Crowdsale)
                    .Include(p => p.Likes)
                    .Include(p => p.Page).ThenInclude(p => p.ShortDescription).ThenInclude(sh => sh.Translations)
                    .Include(p => p.Page).ThenInclude(p => p.PageCovers).ThenInclude(sh => sh.Translations)
                    .AsQueryable();

            var projects = new List<Project>();
            var totalCount = 0;

            if (request.LandingOnly)
            {
                projectsQuery = projectsQuery.Where(q => q.IsVisibleOnLanding == true);
            }
            else
            {
                projectsQuery = projectsQuery.Where(p =>
                    p.Status.Key != Domain.ProjectManagement.Constants.ProjectStatuses.New
                    && p.Status.Key != Domain.ProjectManagement.Constants.ProjectStatuses.Declined
                    && p.Status.Key != Domain.ProjectManagement.Constants.ProjectStatuses.UnderReview && p.IsVisible);
                if (!string.IsNullOrWhiteSpace(request.Status))
                {
                    projectsQuery = ProjectsStatusFilter(projectsQuery, request.Status);
                }
                else
                {
                    var featured = ProjectsStatusFilter(projectsQuery,
                        Domain.ProjectManagement.Constants.MarketplaceStatuses.Featured);
                    var privateSale = ProjectsStatusFilter(projectsQuery,
                        Domain.ProjectManagement.Constants.MarketplaceStatuses.PrivateSale);
                    var publicSale = ProjectsStatusFilter(projectsQuery,
                        Domain.ProjectManagement.Constants.MarketplaceStatuses.PublicSale);
                    var preSale = ProjectsStatusFilter(projectsQuery,
                        Domain.ProjectManagement.Constants.MarketplaceStatuses.PreSale);
                    var upcoming = ProjectsStatusFilter(projectsQuery,
                        Domain.ProjectManagement.Constants.MarketplaceStatuses.Upcoming);
                    var closedProjects = ProjectsStatusFilter(projectsQuery,
                        Domain.ProjectManagement.Constants.MarketplaceStatuses.Closed);

                    projectsQuery =
                        featured.Concat(
                            privateSale.Concat(preSale.Concat(publicSale.Concat(upcoming.Concat(closedProjects)))));
                }
            }

            if (!string.IsNullOrWhiteSpace(request.TextSearch))
            {
                projectsQuery = projectsQuery.Where(p => EF.Functions.Like(p.Title, $"%{request.TextSearch}%"));
            }
            
            projects = await projectsQuery
                .OrderByDescending(r => r.Status.Order)
                .ThenByDescending(p => p.Order)
                .ThenByDescending(p => p.StartDate)
                .AsNoTracking()
                .Skip(request.Skip).Take(request.Take)
                .ToListAsync(cancellationToken);
            totalCount = await projectsQuery.CountAsync(cancellationToken: cancellationToken);
            
            var dtos = new List<ProjectDTO>();
            foreach (var project in projects)
            {
                var dto = await _aggregator.FillInDTOAsync(project);
                dtos.Add(dto);
            }

            return new GetProjectsQueryResponse
            {
                Entities = dtos,
                Total = totalCount
            };
        }

        private IQueryable<Project> ProjectsStatusFilter(IQueryable<Project> projectsQuery, string statusKey)
        {
            if (string.IsNullOrWhiteSpace(statusKey))
            {
                return projectsQuery;
            }
            
            var upcomingStatusKey = Domain.ProjectManagement.Constants.MarketplaceStatuses.Upcoming;
            var featured = Domain.ProjectManagement.Constants.MarketplaceStatuses.Featured;
            var privateStatusKey = Domain.ProjectManagement.Constants.MarketplaceStatuses.PrivateSale;
            var publicSaleStatusKey = Domain.ProjectManagement.Constants.MarketplaceStatuses.PublicSale;
            var preSaleStatusKey = Domain.ProjectManagement.Constants.MarketplaceStatuses.PreSale;

            var approvedStatusKey = Domain.ProjectManagement.Constants.ProjectStatuses.Approved;
            var inProgressStatusKey = Domain.ProjectManagement.Constants.ProjectStatuses.InProgress;
            var closedStatusKey = Domain.ProjectManagement.Constants.ProjectStatuses.Closed;
            
            var pendingStageStatus = Domain.ProjectManagement.Constants.StageStatuses.Pending;
            var activeStageStatus = Domain.ProjectManagement.Constants.StageStatuses.Active;

            var query = projectsQuery;
            
            if (statusKey == upcomingStatusKey)
            {
                query = projectsQuery
                    .Where(p =>
                        p.Status.Key == approvedStatusKey || p.Status.Key == inProgressStatusKey
                        && p.Stages.Any(s => s.Status.Key == pendingStageStatus) && 
                        p.Stages.All(s => s.Status.Key != activeStageStatus));
            }
            if (statusKey == closedStatusKey)
            {
                query = projectsQuery.Where(p => p.Status.Key == closedStatusKey);
            }
            if (statusKey == privateStatusKey)
            {
                query = projectsQuery.Where(p =>
                    p.Status.Key == inProgressStatusKey
                    && p.Stages.Any(s =>
                        s.Status.Key == activeStageStatus && s.Type == StageType.Private));
            }
            if (statusKey == publicSaleStatusKey)
            {
                query = projectsQuery.Where(p => 
                    p.Status.Key == inProgressStatusKey
                    && p.Stages.Any(s =>
                        s.Status.Key == activeStageStatus && s.Type == StageType.Public));
            }
            if (statusKey == preSaleStatusKey)
            {
                query = projectsQuery.Where(p => 
                    p.Status.Key == inProgressStatusKey
                    && p.Stages.Any(s =>
                        s.Status.Key == activeStageStatus && s.Type == StageType.PreSale));
            }
            if (statusKey == featured)
            {
                query = projectsQuery.Where(p => p.IsFeatured &&  p.Status.Key == inProgressStatusKey && p.Stages.Any(s =>
                    s.Status.Key == activeStageStatus));
            }
            
            return query.OrderByDescending (p => p.StartDate.Value);
        }
    }
}