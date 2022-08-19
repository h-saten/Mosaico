using System;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.Exceptions;
using Mosaico.Application.ProjectManagement.Services;
using Mosaico.BackgroundJobs.Base;
using Mosaico.BackgroundJobs.Hangfire.Abstractions;
using Mosaico.Base.Tools;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities;
using Mosaico.Events.Base;
using Mosaico.Events.ProjectManagement;
using Serilog;

namespace Mosaico.Application.ProjectManagement.BackgroundJobs
{
    [BackgroundJob(Constants.Jobs.StageActivationJob, IsRecurring = true, Cron = "*/30 * * * *")]
    public class StageActivationJob : HangfireBackgroundJobBase
    {
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILogger _logger;
        private readonly IProjectDbContext _projectDbContext;
        private readonly IDateTimeProvider _provider;
        private readonly IStageService _stageService;

        public StageActivationJob(IProjectDbContext projectDbContext, IEventPublisher eventPublisher, IEventFactory eventFactory, 
            IDateTimeProvider provider, IStageService stageService, ILogger logger = null)
        {
            _projectDbContext = projectDbContext;
            _eventPublisher = eventPublisher;
            _eventFactory = eventFactory;
            _provider = provider;
            _stageService = stageService;
            _logger = logger;
        }
        
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        [DisableConcurrentExecution(timeoutInSeconds: 10 * 60)]
        public override async Task ExecuteAsync(object parameters = null)
        {
            var now = _provider.Now();
            _logger?.Verbose($"Job Stage Deployer started at {now:g}");
            var projects = await _projectDbContext.Projects
                .Include(p => p.Stages).ThenInclude(s => s.Status)
                .Include(p => p.Status)
                .Include(p => p.Crowdsale)
                .Where(p => p.Status.Key != Domain.ProjectManagement.Constants.ProjectStatuses.New && 
                                p.Status.Key != Domain.ProjectManagement.Constants.ProjectStatuses.UnderReview)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            var stageInProgress = await _projectDbContext.StageStatuses.FirstOrDefaultAsync(s =>
                s.Key == Domain.ProjectManagement.Constants.StageStatuses.Active);
            var stagePending = await _projectDbContext.StageStatuses.FirstOrDefaultAsync(s =>
                s.Key == Domain.ProjectManagement.Constants.StageStatuses.Pending);
            var projectInProgress = await _projectDbContext.ProjectStatuses.FirstOrDefaultAsync(p =>
                p.Key == Domain.ProjectManagement.Constants.ProjectStatuses.InProgress);

            Exception currentException = default;
            
            foreach (var project in projects)
            {
                try
                {
                    var currentStage = project.ActiveStage();
                    if (currentStage == null) continue;
                    if (currentStage.Status.Key == Domain.ProjectManagement.Constants.StageStatuses.Pending)
                    {
                        if (now >= currentStage.StartDate)
                        {
                            await _stageService.StartStageAsync(currentStage);
                            if (project.Status.Key != Domain.ProjectManagement.Constants.ProjectStatuses.InProgress)
                            {
                                await UpdateProjectStatusAsync(project, projectInProgress);
                            }

                            await SendEventsAsync(currentStage);
                        }
                    }
                    else if (currentStage.Status.Key == Domain.ProjectManagement.Constants.StageStatuses.Active)
                    {
                        if (now > currentStage.EndDate)
                        {
                            await _stageService.SetStageClosed(currentStage);
                            var nextStage = currentStage.Project.NextStage(currentStage.Order + 1);
                            if (nextStage == null || nextStage.Status.Key !=
                                Domain.ProjectManagement.Constants.StageStatuses.Pending)
                            {
                                var projectClosedStatus = await _projectDbContext.ProjectStatuses.FirstOrDefaultAsync(
                                    p =>
                                        p.Key == Domain.ProjectManagement.Constants.ProjectStatuses.Closed);
                                project.SetStatus(projectClosedStatus);
                                await _projectDbContext.SaveChangesAsync();
                            }
                        }
                    }
                    else
                    {
                        if (now >= currentStage.StartDate && now < currentStage.EndDate)
                        {
                            await _stageService.StartStageAsync(currentStage);
                            if (project.Status.Key != Domain.ProjectManagement.Constants.ProjectStatuses.InProgress)
                            {
                                await UpdateProjectStatusAsync(project, projectInProgress);
                            }

                            await SendEventsAsync(currentStage);
                        }
                    }
                }
                catch (Exception ex)
                {
                    currentException = ex;
                }
            }

            if (currentException != default)
            {
                throw currentException;
            }
        }

        private async Task UpdateProjectStatusAsync(Project project, ProjectStatus projectStatus)
        {
            project.SetStatus(projectStatus);
            await _projectDbContext.SaveChangesAsync();
            _logger?.Verbose($"Updated status of project {project.Id} to {projectStatus.Key}");
        }
        
        private async Task SendEventsAsync(Stage stage)
        {
            try
            {
                var @event = _eventFactory.CreateEvent(Events.ProjectManagement.Constants.EventPaths.Projects,
                    new StageDeployedEvent(stage.Id));
                await _eventPublisher.PublishAsync(Events.ProjectManagement.Constants.EventPaths.Projects, @event);
            }
            catch (Exception ex)
            {
                _logger?.Error($"{ex.Message} + {ex.StackTrace}");
            }
        }
    }
}