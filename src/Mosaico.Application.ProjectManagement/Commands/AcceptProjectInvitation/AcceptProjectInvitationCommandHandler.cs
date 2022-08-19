using System;
using System.Linq;
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
using Mosaico.SDK.Identity.Abstractions;
using Serilog;

namespace Mosaico.Application.ProjectManagement.Commands.AcceptProjectInvitation
{
    public class AcceptProjectInvitationCommandHandler : IRequestHandler<AcceptProjectInvitationCommand, Guid>
    {
        private readonly IProjectDbContext _projectDbContext;
        private readonly ILogger _logger;
        private readonly ICurrentUserContext _currentUserContext;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly IUserManagementClient _managementClient;
        
        public AcceptProjectInvitationCommandHandler(IProjectDbContext projectDbContext, ICurrentUserContext currentUserContext, IUserManagementClient managementClient, IEventFactory eventFactory, IEventPublisher eventPublisher, ILogger logger = null)
        {
            _projectDbContext = projectDbContext;
            _currentUserContext = currentUserContext;
            _managementClient = managementClient;
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _logger = logger;
        }

        public async Task<Guid> Handle(AcceptProjectInvitationCommand request, CancellationToken cancellationToken)
        {
            using (var transaction = _projectDbContext.BeginTransaction())
            {
                try
                {
                    var user = await _managementClient.GetUserAsync(_currentUserContext.UserId.ToString(), cancellationToken);
                    var now = DateTimeOffset.UtcNow;
                    var invitations =
                        await _projectDbContext.ProjectMembers.Where(p => !p.IsAccepted
                             && p.Email == user.Email && p.ExpiresAt > now).ToListAsync(cancellationToken);
                    
                    var invitation = invitations.FirstOrDefault(i => i.AuthorizationCode == request.AuthorizationCode);
                    
                    if (invitation == null)
                    {
                        throw new InvitationNotFoundException("");
                    }
                    _logger?.Verbose($"Invitation was found");
                    
                    if (invitation.IsAccepted || DateTimeOffset.UtcNow > invitation.ExpiresAt)
                    {
                        throw new ProjectInvitationExpiredException();
                    }
                    _logger?.Verbose($"Invitation is still valid");
                    
                    invitation.UserId = _currentUserContext.UserId;
                    invitation.AcceptedAt = DateTimeOffset.UtcNow;
                    invitation.IsAccepted = true;
                    
                    _logger?.Verbose($"Updating invitation in database");
                    await _projectDbContext.SaveChangesAsync(cancellationToken);
                    _logger?.Verbose($"Sending events");
                    await PublishEventsAsync(invitation.ProjectId, invitation.Id);
                    await transaction.CommitAsync(cancellationToken);
                    return invitation.ProjectId;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }
        }

        private async Task PublishEventsAsync(Guid projectId, Guid invitationId)
        {
            var acceptedInvitationEvent = _eventFactory.CreateEvent(Events.ProjectManagement.Constants.EventPaths.Projects, new ProjectInvitationAcceptedEvent(projectId, invitationId));
            await _eventPublisher.PublishAsync(Events.ProjectManagement.Constants.EventPaths.Projects, acceptedInvitationEvent);
            var membershipUpdatedEvent = _eventFactory.CreateEvent(Events.ProjectManagement.Constants.EventPaths.Projects,
                new ProjectMemberUpdatedEvent(projectId, invitationId));
            await _eventPublisher.PublishAsync(Events.ProjectManagement.Constants.EventPaths.Projects, membershipUpdatedEvent);
        }
    }
}