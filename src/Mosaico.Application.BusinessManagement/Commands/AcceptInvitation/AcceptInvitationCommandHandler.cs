using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Authorization.Base;
using Mosaico.Domain.BusinessManagement.Abstractions;
using Mosaico.Domain.BusinessManagement.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.BusinessManagement;
using Mosaico.SDK.Identity.Abstractions;
using Mosaico.SDK.Identity.Exceptions;
using Serilog;

namespace Mosaico.Application.BusinessManagement.Commands.AcceptInvitation
{
    public class AcceptInvitationCommandHandler : IRequestHandler<AcceptInvitationCommand, Guid>
    {
        private readonly ILogger _logger;
        private readonly IBusinessDbContext _businessDbContext;
        private readonly ICurrentUserContext _currentUserContext;
        private readonly IUserManagementClient _userManagementClient;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;

        public AcceptInvitationCommandHandler(IBusinessDbContext businessDbContext, ICurrentUserContext currentUserContext, IUserManagementClient userManagementClient, IEventFactory eventFactory, IEventPublisher eventPublisher, ILogger logger = null)
        {
            _businessDbContext = businessDbContext;
            _currentUserContext = currentUserContext;
            _userManagementClient = userManagementClient;
            _logger = logger;
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
        }

        public async Task<Guid> Handle(AcceptInvitationCommand request, CancellationToken cancellationToken)
        {
            using (var transaction = _businessDbContext.BeginTransaction())
            {
                try
                {
                    var now = DateTimeOffset.UtcNow;
                    
                    var invitations = await _businessDbContext.TeamMembers
                        .Where(i => !i.IsAccepted && i.ExpiresAt > now).ToListAsync(cancellationToken);
                    var invitation = invitations.FirstOrDefault(i => i.AuthorizationCode == request.Code);
                    
                    if (invitation == null)
                    {
                        throw new InvitationNotFoundException(request.Code);
                    }
                    
                    if (invitation.IsAccepted)
                    {
                        throw new InvitationAlreadyAcceptedException(request.Code);
                    }

                    if (invitation.ExpiresAt.HasValue && DateTimeOffset.UtcNow > invitation.ExpiresAt.Value)
                    {
                        throw new InvitationAlreadyExpiredException(request.Code);
                    }
                    
                    invitation.AcceptedAt = DateTime.UtcNow;
                    invitation.IsAccepted = true;

                    var user = await _userManagementClient.GetUserAsync(_currentUserContext.UserId, cancellationToken);
                    if (user == null)
                    {
                        throw new UserNotFoundException(_currentUserContext.UserId);
                    }
                    
                    if (invitation.Email != user.Email)
                    {
                        throw new InvalidInvitationEmailException(user.Id);
                    }
                    
                    invitation.UserId = user.Id;
                    _businessDbContext.TeamMembers.Update(invitation);
                    await _businessDbContext.SaveChangesAsync(cancellationToken);
                    await PublishEvents(invitation.CompanyId,invitation.Id);
                    await transaction.CommitAsync(cancellationToken);
                    return invitation.CompanyId;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }
        }

        private async Task PublishEvents(Guid companyId, Guid invitationId)
        {
            var e = _eventFactory.CreateEvent(Events.BusinessManagement.Constants.EventPaths.Companies,
                new CompanyInvitationUpdatedEvent(companyId, invitationId));
            await _eventPublisher.PublishAsync(Events.BusinessManagement.Constants.EventPaths.Companies, e);
        }
    }
}