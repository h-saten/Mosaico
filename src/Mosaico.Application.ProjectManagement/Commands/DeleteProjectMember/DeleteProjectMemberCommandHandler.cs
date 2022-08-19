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

namespace Mosaico.Application.ProjectManagement.Commands.DeleteProjectMember
{
    public class DeleteProjectMemberCommandHandler : IRequestHandler<DeleteProjectMemberCommand>
    {
        private readonly IProjectDbContext _projectDbContext;
        private readonly ILogger _logger;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly ICurrentUserContext _currentUserContext;

        public DeleteProjectMemberCommandHandler(IProjectDbContext projectDbContext, ICurrentUserContext currentUserContext, IEventFactory eventFactory, IEventPublisher eventPublisher, ILogger logger = null)
        {
            _projectDbContext = projectDbContext;
            _currentUserContext = currentUserContext;
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteProjectMemberCommand request, CancellationToken cancellationToken)
        {
            using (var transaction = _projectDbContext.BeginTransaction())
            {
                try
                {
                    var projectMember = await _projectDbContext.ProjectMembers.Include(p => p.Project)
                        .FirstOrDefaultAsync(p => p.Id == request.ProjectMemberId && p.ProjectId == request.ProjectId, cancellationToken);
                    if (!string.IsNullOrEmpty(projectMember.UserId))
                    {
                        if (projectMember.UserId == projectMember.Project.CreatedById)
                        {
                            throw new CannotDeleteProjectCreatorException();
                        }

                        if (projectMember.UserId == _currentUserContext.UserId)
                        {
                            throw new CannotSelfDeleteException();
                        }
                    }

                    _projectDbContext.ProjectMembers.Remove(projectMember);
                    await _projectDbContext.SaveChangesAsync(cancellationToken);
                    _logger?.Verbose($"Sending events");
                    await PublishEventAsync(projectMember.ProjectId, projectMember.UserId, projectMember.Email);
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

        private async Task PublishEventAsync(Guid projectId, string userId, string email)
        {
            //TODO: create event handler and send email to deleted user if userId.HasValue()
            var e = _eventFactory.CreateEvent(Events.ProjectManagement.Constants.EventPaths.Projects,
                new ProjectMemberDeletedEvent(projectId, email, userId));
            await _eventPublisher.PublishAsync(Events.ProjectManagement.Constants.EventPaths.Projects, e);
        }
    }
}