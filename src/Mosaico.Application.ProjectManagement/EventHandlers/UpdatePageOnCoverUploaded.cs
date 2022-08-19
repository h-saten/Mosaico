using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.Queries.TokenPage.GetTokenPage;
using Mosaico.Cache.Base.Abstractions;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities.TokenPage;
using Mosaico.Events.Base;
using Mosaico.Events.ProjectManagement;
using Mosaico.Integration.SignalR.Abstractions;
using Serilog;

namespace Mosaico.Application.ProjectManagement.EventHandlers
{
    [EventInfo(nameof(UpdatePageOnCoverUploaded), "projects:api")]
    [EventTypeFilter(typeof(PageCoverUploaded))]
    public class UpdatePageOnCoverUploaded : EventHandlerBase
    {
        private readonly IProjectDbContext _projectDbContext;
        private readonly ITokenPageDispatcher _dispatcher;
        private readonly ICacheClient _cacheClient;
        private readonly ILogger _logger;

        public UpdatePageOnCoverUploaded(IProjectDbContext projectDbContext, ITokenPageDispatcher dispatcher, ICacheClient cacheClient, ILogger logger = null)
        {
            _projectDbContext = projectDbContext;
            _dispatcher = dispatcher;
            _cacheClient = cacheClient;
            _logger = logger;
        }

        public override async Task HandleAsync(CloudEvent @event)
        {
            var eventData = @event?.GetData<PageCoverUploaded>();
            if (eventData != null)
            {
                var pageCover = await _projectDbContext.PageCovers.FirstOrDefaultAsync(p => p.PageId == eventData.PageId);
                if (pageCover != null)
                {
                    pageCover.UpdateTranslation(eventData.Url, eventData.Language);
                    await _projectDbContext.SaveChangesAsync();
                }
                else
                {
                    _logger?.Warning($"Page {eventData.PageId} not found to update cover.");
                    var key = Guid.NewGuid().ToString();
                    pageCover = new PageCover
                    {
                        Key = key,
                        Title = key,
                        PageId = eventData.PageId,
                        PrimaryColor = Constants.DefaultColors.PrimaryColor,
                        SecondaryColor = Constants.DefaultColors.SecondaryColor,
                        CoverColor = Constants.DefaultColors.CoverColor
                    };
                    pageCover.UpdateTranslation(eventData.Url, eventData.Language);
                    _projectDbContext.PageCovers.Add(pageCover);
                    await _projectDbContext.SaveChangesAsync();
                }

                await _cacheClient.CleanAsync($"{nameof(GetTokenPageQuery)}_{eventData.PageId}_*");
                await _dispatcher.DispatchCoverUpdatedAsync(eventData.Url);
            }
        }
    }
}