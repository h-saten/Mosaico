using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.Queries.GetProject;
using Mosaico.Cache.Base.Abstractions;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.Wallet;
using Mosaico.Integration.SignalR.Abstractions;
using Serilog;

namespace Mosaico.Application.ProjectManagement.EventHandlers
{
    [EventInfo(nameof(UpdateProjectLogoUrlOnFileUploaded), "wallets:api")]
    [EventTypeFilter(typeof(TokenLogoUploaded))]
    public class UpdateProjectLogoUrlOnFileUploaded : EventHandlerBase
    {
        private readonly ILogger _logger;
        private readonly IProjectDbContext _projectDbContext;
        private readonly ICacheClient _cacheClient;
        private readonly ITokenPageDispatcher _pageDispatcher;
        
        public UpdateProjectLogoUrlOnFileUploaded(IProjectDbContext projectDbContext, ICacheClient cacheClient, ITokenPageDispatcher pageDispatcher, ILogger logger = null)
        {
            _projectDbContext = projectDbContext;
            _cacheClient = cacheClient;
            _pageDispatcher = pageDispatcher;
            _logger = logger;
        }

        public override async Task HandleAsync(CloudEvent @event)
        {
            var projectEvent = @event?.GetData<TokenLogoUploaded>();
            if (projectEvent != null)
            {
                var project = await _projectDbContext.Projects.FirstOrDefaultAsync(p => p.TokenId == projectEvent.TokenId);
                if (project == null)
                {
                    throw new ProjectNotFoundException(projectEvent.TokenId);
                }
                _logger?.Verbose($"Project {project.Id} was found. Attempting to change logo value to {projectEvent.LogoUrl}");
                project.LogoUrl = projectEvent.LogoUrl;
                await _projectDbContext.SaveChangesAsync();
                _logger?.Verbose($"Project logo was successfully changed");
                await _cacheClient.CleanAsync(new List<string>
                {
                    $"{nameof(GetProjectQuery)}_{project.Id}",
                    $"{nameof(GetProjectQuery)}_{project.Slug}"
                });
                await _pageDispatcher.DispatchLogoUpdatedAsync(project.LogoUrl);
            }
        }
    }
}