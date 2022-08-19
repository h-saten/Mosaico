using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.Exceptions;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.ProjectManagement;
using Serilog;

namespace Mosaico.Application.ProjectManagement.Commands.AddProjectMember
{
    public class AddProjectMemberCommandHandler : IRequestHandler<AddProjectMemberCommand>
    {
        private readonly IProjectDbContext _context;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILogger _logger;
        
        public AddProjectMemberCommandHandler(IProjectDbContext context, IEventFactory eventFactory, IEventPublisher eventPublisher, ILogger logger = null)
        {
            _context = context;
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _logger = logger;
        }

        public async Task<Unit> Handle(AddProjectMemberCommand request, CancellationToken cancellationToken)
        {
            using (var transaction = _context.BeginTransaction())
            {
                try
                {
                    _logger?.Verbose($"Trying to add new member to the project");
            
                    var project = await _context.Projects.Include(p => p.Members).FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken);
                    if (project == null)
                    {
                        throw new ProjectNotFoundException(request.ProjectId);
                    }
            
                    _logger?.Verbose($"Project {project.Title}/{project.Id} was found");
            
                    var existingInvitation = project.Members.FirstOrDefault(m => m.Email == request.Email);
                    if (existingInvitation != null)
                    {
                        throw new ProjectInvitationAlreadySentException(request.Email);
                    }
                    _logger?.Verbose($"There are no existing invitations");
            
                    var role = await _context.Roles.FirstOrDefaultAsync(r => r.Key == request.Role, cancellationToken);
                    if (role == null)
                    {
                        throw new ProjectRoleNotFoundException(request.Role);
                    }
                    _logger?.Verbose($"Requested role {request.Role} is valid");

                    if (project.Members.Count >= Constants.ProjectMemberLimit)
                    {
                        throw new ProjectMemberLimitExceededException();
                    }
                    
                    _logger?.Verbose($"Team member limit is not exceeded");
                    
                    project.Members.Add(new ProjectMember
                    {
                        Email = request.Email,
                        Role = role,
                        RoleId = role.Id,
                        AuthorizationCode = Guid.NewGuid().ToString(),
                        ExpiresAt = DateTimeOffset.UtcNow.AddDays(14),
                        IsInvitationSent = false
                    });
                    _logger?.Verbose($"Attempting to save result");
                    await _context.SaveChangesAsync(cancellationToken);
                    _logger?.Verbose($"Sending events");
                    await PublishEventAsync(project.Id, request.Email);
                    await transaction.CommitAsync(cancellationToken);
                    return Unit.Value;
                }
                catch (Exception)
                {
                    _logger?.Verbose($"Rolling back transaction");
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }
        }

        private async Task PublishEventAsync(Guid projectId, string email)
        {
            var e = _eventFactory.CreateEvent(Events.ProjectManagement.Constants.EventPaths.Projects, new ProjectMemberAddedEvent(projectId, email));
            await _eventPublisher.PublishAsync(Events.ProjectManagement.Constants.EventPaths.Projects, e);
        }
        
    }
}