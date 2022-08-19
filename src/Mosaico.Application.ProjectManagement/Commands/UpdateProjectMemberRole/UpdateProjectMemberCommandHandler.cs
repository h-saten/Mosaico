using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.Exceptions;
using Mosaico.Authorization.Base;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.ProjectManagement;
using Serilog;

namespace Mosaico.Application.ProjectManagement.Commands.UpdateProjectMemberRole
{
    public class UpdateProjectMemberCommandHandler : IRequestHandler<UpdateProjectMemberCommand>
    {
        private readonly IProjectDbContext _projectDbContext;
        private readonly ILogger _logger;
        private readonly ICurrentUserContext _currentUserContext;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        
        public UpdateProjectMemberCommandHandler(IProjectDbContext projectDbContext, ICurrentUserContext currentUserContext, IEventFactory eventFactory, IEventPublisher eventPublisher, ILogger logger = null)
        {
            _projectDbContext = projectDbContext;
            _currentUserContext = currentUserContext;
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateProjectMemberCommand request, CancellationToken cancellationToken)
        {
            var member = await _projectDbContext.ProjectMembers.Include(r => r.Role).FirstOrDefaultAsync(p => p.ProjectId == request.ProjectId && p.Id == request.MemberId, cancellationToken);
            if (member == null)
            {
                throw new ProjectMemberNotFoundException(request.MemberId);
            }
            
            _logger?.Verbose($"Member was successfully retrieved");
            var role = await _projectDbContext.Roles.FirstOrDefaultAsync(r => r.Key == request.Role, cancellationToken);
            if (role == null)
            {
                throw new ProjectRoleNotFoundException(request.Role);
            }

            if (member.UserId == _currentUserContext.UserId)
            {
                throw new CannotChangeOwnRoleException();
            }
            
            _logger?.Verbose($"Role was successfully retrieved");
            if (member.Role.Key != role.Key)
            {
                _logger?.Verbose($"User {member.UserId} had different role for the project {member.ProjectId}. Changing to {request.Role}");
                member.Role = role;
                member.RoleId = role.Id;
                _logger?.Verbose("Saving changes");
                await _projectDbContext.SaveChangesAsync(cancellationToken);
                _logger?.Verbose($"Sending events");
                await PublishEvents(member.ProjectId, member.Id);
            }
            
            return Unit.Value;
        }

        private async Task PublishEvents(Guid projectId, Guid memberId)
        {
            var e = _eventFactory.CreateEvent(Events.ProjectManagement.Constants.EventPaths.Projects,
                new ProjectMemberUpdatedEvent(projectId, memberId));
            await _eventPublisher.PublishAsync(Events.ProjectManagement.Constants.EventPaths.Projects, e);
        }
    }
}