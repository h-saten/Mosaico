using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.Exceptions;
using Mosaico.Base.Tools;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Serilog;

namespace Mosaico.Application.ProjectManagement.Services
{
    public class StageService : IStageService
    {
        private readonly ILogger _logger;
        private readonly IDateTimeProvider _provider;
        private readonly IProjectDbContext _projectDbContext;

        public StageService(ILogger logger, IDateTimeProvider provider, IProjectDbContext projectDbContext)
        {
            _logger = logger;
            _provider = provider;
            _projectDbContext = projectDbContext;
        }

        public async Task StartStageAsync(Stage stage)
        {
            var now = _provider.Now();
            if (stage == null)
            {
                throw new StageNotFoundException(Guid.Empty);
            }
            
            //we cannot start stage without an end date
            if (!stage.EndDate.HasValue || stage.EndDate <= now)
            {
                _logger?.Error($"Stage {stage.Id} has no end date");
                throw new StageEndDateException(stage.Id.ToString());
            }
            
            //we need to make sure that previous stage was closed previously
            var anotherStage = stage.Project.Stages.FirstOrDefault(s =>
                s.Id != stage.Id &&
                s.Status.Key == Domain.ProjectManagement.Constants.StageStatuses.Active);
            
            if (anotherStage != null)
            {
                _logger?.Error($"Project {stage.ProjectId} has other stage in progress...cannot active stage");
                throw new StageStillActiveException(stage.Id.ToString(), anotherStage.Id.ToString());
            }
            
            var stageActiveStatus = await _projectDbContext.StageStatuses.FirstOrDefaultAsync(s =>
                s.Key == Domain.ProjectManagement.Constants.StageStatuses.Active);
           
            await UpdateStageStatusAsync(stage, stageActiveStatus);
        }

        public async Task SetStagePending(Stage stage)
        {
            if (stage.Status.Key == Domain.ProjectManagement.Constants.StageStatuses.Closed)
            {
                throw new StageNotFoundException(Guid.Empty);
            }
            var pendingStatus = await _projectDbContext.StageStatuses.FirstOrDefaultAsync(s =>
                s.Key == Domain.ProjectManagement.Constants.StageStatuses.Pending);
           
            await UpdateStageStatusAsync(stage, pendingStatus);
        }
        
        public async Task SetStageClosed(Stage stage)
        {
            var closed = await _projectDbContext.StageStatuses.FirstOrDefaultAsync(s =>
                s.Key == Domain.ProjectManagement.Constants.StageStatuses.Closed);
           
            await UpdateStageStatusAsync(stage, closed);
        }
        
        protected async Task UpdateStageStatusAsync(Stage stage, StageStatus status)
        {
            stage.SetStatus(status);
            await _projectDbContext.SaveChangesAsync();
            _logger?.Verbose($"Stage {stage.Id} was updated with status {status.Key}");
        }
    }
}