using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Events.Base;
using Mosaico.Events.ProjectManagement;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mosaico.Application.ProjectManagement.Commands.DisplayArticle
{
    public class DisplayArticleCommandHandler : IRequestHandler<DisplayArticleCommand>
    {
        private readonly IProjectDbContext _projectDbContext;
        private readonly ILogger _logger;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        public DisplayArticleCommandHandler(IProjectDbContext projectDbContext, IEventFactory eventFactory, IEventPublisher eventPublisher, ILogger logger = null)
        {
            _projectDbContext = projectDbContext;
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _logger = logger;
        }

        public async Task<Unit> Handle(DisplayArticleCommand request, CancellationToken cancellationToken)
        {
            using (var transaction = _projectDbContext.BeginTransaction())
            {
                try
                {
                    var article = await _projectDbContext.Articles
                        .FirstOrDefaultAsync(p => p.Id == request.ArticleId, cancellationToken);

                    article.Hidden = false;
                    await _projectDbContext.SaveChangesAsync(cancellationToken);
                    _logger?.Verbose($"Sending events");
                    await PublishEventAsync(request.ArticleId);
                    _logger?.Verbose("Commiting transaction");
                    await transaction.CommitAsync(cancellationToken);
                    return Unit.Value;
                }
                catch (Exception)
                {
                    _logger?.Verbose($"Rollback transaction");
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }
        }

        private async Task PublishEventAsync(Guid articleId)
        {
            var e = _eventFactory.CreateEvent(Events.ProjectManagement.Constants.EventPaths.Projects,
                new ProjectArticleHiddenEvent(articleId));
            await _eventPublisher.PublishAsync(Events.ProjectManagement.Constants.EventPaths.Projects, e);
        }
    }
}
