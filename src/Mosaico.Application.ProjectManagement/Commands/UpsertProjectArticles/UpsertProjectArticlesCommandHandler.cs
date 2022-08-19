using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Authorization.Base;
using Mosaico.Base.Exceptions;
using Mosaico.Base.Tools;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.ProjectManagement;
using Mosaico.SDK.Identity.Abstractions;
using Serilog;

namespace Mosaico.Application.ProjectManagement.Commands.UpsertProjectArticles
{
    public class UpsertProjectArticlesCommandHandler : IRequestHandler<UpsertProjectArticlesCommand, Guid>
    {
        private readonly ILogger _logger;
        private readonly IProjectDbContext _projectDbContext;
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventFactory _eventFactory;
        private readonly IUserManagementClient _managementClient;
        private readonly IDateTimeProvider _dateTimeProvider;

        public UpsertProjectArticlesCommandHandler(ILogger logger, IProjectDbContext projectDbContext, IEventPublisher eventPublisher, IEventFactory eventFactory,  IUserManagementClient managementClient, IDateTimeProvider dateTimeProvider)
        {
            _logger = logger;
            _projectDbContext = projectDbContext;
            _eventPublisher = eventPublisher;
            _eventFactory = eventFactory;
            _managementClient = managementClient;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Guid> Handle(UpsertProjectArticlesCommand request, CancellationToken cancellationToken)
        {
            _logger?.Verbose($"Attempting to create project article");
            
            var project = await _projectDbContext.Projects.Include(x=>x.Members).Include(x=>x.Articles)
                .FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken);
            if (project == null)
            {
                throw new ProjectNotFoundException(request.ProjectId);
            }
            _logger?.Verbose($"Project {request.ProjectId} was found");

            var currentUser = await _managementClient.GetCurrentUserAsync(cancellationToken);
            var currentUserName = "";
            if (!string.IsNullOrWhiteSpace(currentUser.FirstName))
                currentUserName = currentUser.FirstName + " " + currentUser.LastName;
            else currentUserName = currentUser.Email;

            request.Date ??= _dateTimeProvider.Now();
            
            var article = project.Articles.FirstOrDefault(d => d.Id == request.ArticleId);
            if (article == null)
            {
                if(project.Articles.Count >= Constants.ArticleLimit)
                {
                    throw new LimitExceededException(nameof(Article));
                }
                article = new Article
                {
                    VisibleText = request.VisibleText,
                    AuthorPhoto = request.AuthorPhoto,
                    CoverPicture = request.CoverPicture,
                    Date = request.Date,
                    Link = request.Link,
                    ProjectId = request.ProjectId,
                    Project = project,
                    AuthorName = !string.IsNullOrEmpty(request.Name) ? request.Name : currentUserName,
                    Hidden = false
                };
                _projectDbContext.Articles.Add(article);
            }
            else
            {
                article.VisibleText = request.VisibleText;
                article.AuthorPhoto = request.AuthorPhoto;
                article.CoverPicture = request.CoverPicture;
                article.Date = request.Date;
                article.Link = request.Link;
                article.ProjectId = request.ProjectId;
                article.AuthorName = !string.IsNullOrEmpty(request.Name) ? request.Name : currentUserName;
            }

            await _projectDbContext.SaveChangesAsync(cancellationToken);

            if (request.CoverId != Guid.Empty && request.PhotoId != Guid.Empty)
            {
                await PublishEvents(article.Id, request.CoverId, request.PhotoId);
            }
            return article.Id;
        }

        private async Task PublishEvents(Guid articleId, Guid coverId, Guid photoId)
        {
            var e = _eventFactory.CreateEvent(Events.ProjectManagement.Constants.EventPaths.Projects, new ProjectArticleUpdated(articleId, coverId, photoId));
            await _eventPublisher.PublishAsync(e);
        }

    }
}