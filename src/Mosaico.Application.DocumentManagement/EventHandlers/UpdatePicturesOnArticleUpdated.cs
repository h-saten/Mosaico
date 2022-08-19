using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.DocumentManagement.Abstractions;
using Mosaico.Events.Base;
using Mosaico.Events.ProjectManagement;
using Serilog;

namespace Mosaico.Application.DocumentManagement.EventHandlers
{
    [EventInfo(nameof(UpdatePicturesOnArticleUpdated), "projects:api")]
    [EventTypeFilter(typeof(ProjectArticleUpdated))]
    public class UpdatePicturesOnArticleUpdated : EventHandlerBase
    {
        private readonly IDocumentDbContext _documentDbContext;
        private readonly ILogger _logger;

        public UpdatePicturesOnArticleUpdated(IDocumentDbContext documentDbContext, ILogger logger = null)
        {
            _documentDbContext = documentDbContext;
            _logger = logger;
        }

        public override async Task HandleAsync(CloudEvent @event)
        {
            var projectEvent = @event?.GetData<ProjectArticleUpdated>();
            if (projectEvent != null)
            {
                var articleCover = await _documentDbContext.ArticleCovers.FirstOrDefaultAsync(p => p.Id == projectEvent.CoverId);
                if (articleCover != null)
                {

                    articleCover.ArticleId = projectEvent.ArticleId;
                }
                var articlePhoto = await _documentDbContext.ArticlePhotos.FirstOrDefaultAsync(p => p.Id == projectEvent.PhotoId);
                if (articlePhoto != null)
                {

                    articlePhoto.ArticleId = projectEvent.ArticleId;
                }
                await _documentDbContext.SaveChangesAsync();
                _logger?.Verbose($"Article cover and author photo were successfully changed");
            }
        }
    }
}