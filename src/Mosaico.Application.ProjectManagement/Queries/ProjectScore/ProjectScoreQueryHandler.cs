using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Serilog;

namespace Mosaico.Application.ProjectManagement.Queries.ProjectScore
{
    public class ProjectScoreQueryHandler : IRequestHandler<ProjectScoreQuery, ProjectScoreQueryResponse>
    {
        private readonly IProjectDbContext _projectDbContext;
        private readonly ILogger _logger;
        private readonly IValidator<Project> _validator;
        
        public ProjectScoreQueryHandler(IProjectDbContext projectDbContext, IValidator<Project> validator, ILogger logger = null)
        {
            _projectDbContext = projectDbContext;
            _validator = validator;
            _logger = logger;
        }

        public async Task<ProjectScoreQueryResponse> Handle(ProjectScoreQuery request, CancellationToken cancellationToken)
        {
            var project = await _projectDbContext.Projects.FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken);
            if (project == null)
            {
                throw new ProjectNotFoundException(request.ProjectId);
            }
            
            var newStatus = await _projectDbContext.ProjectStatuses.FirstOrDefaultAsync(p => p.Key == Domain.ProjectManagement.Constants.ProjectStatuses.UnderReview, cancellationToken: cancellationToken);
            if (newStatus == null)
            {
                throw new ProjectStatusNotFoundException(Domain.ProjectManagement.Constants.ProjectStatuses.UnderReview);
            }
            project.SetStatus(newStatus);
            
            var validationResult = await _validator.ValidateAsync(project, strategy =>
            {
                strategy.IncludeAllRuleSets();
            }, cancellationToken);
            var criteriaCount = validationResult.RuleSetsExecuted.Length > 0 ? validationResult.RuleSetsExecuted.Length : 1;
            double completedPercentage = 100 - (validationResult.Errors.Count * 100 / criteriaCount);
            return new ProjectScoreQueryResponse
            {
                Score = Math.Floor(completedPercentage),
                Errors = validationResult.Errors.Select(e => e.ErrorCode).ToList()
            };
        }
    }
}