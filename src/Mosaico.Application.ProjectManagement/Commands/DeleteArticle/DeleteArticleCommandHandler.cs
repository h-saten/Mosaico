using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.Exceptions;
using Mosaico.Authorization.Base;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Events.Base;
using Mosaico.Events.ProjectManagement;
using Serilog;

namespace Mosaico.Application.ProjectManagement.Commands.DeleteArticle
{
    public class DeleteArticleCommandHandler : IRequestHandler<DeleteArticleCommand>
    {
        private readonly IProjectDbContext _projectDbContext;
        private readonly ILogger _logger;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;

        public DeleteArticleCommandHandler(IProjectDbContext projectDbContext,  IEventFactory eventFactory, IEventPublisher eventPublisher, ILogger logger = null)
        {
            _projectDbContext = projectDbContext;
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteArticleCommand request, CancellationToken cancellationToken)
        {
            using (var transaction = _projectDbContext.BeginTransaction())
            {
                try
                {
                    var article = await _projectDbContext.Articles
                        .FirstOrDefaultAsync(p => p.Id == request.ArticleId, cancellationToken);

                    _projectDbContext.Articles.Remove(article);
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
                new ProjectArticleDeletedEvent(articleId));
            await _eventPublisher.PublishAsync(Events.ProjectManagement.Constants.EventPaths.Projects, e);
        }
    }
}