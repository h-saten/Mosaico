using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.BusinessManagement.Abstractions;
using Mosaico.Domain.BusinessManagement.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.BusinessManagement;
using Mosaico.SDK.Identity.Abstractions;
using Serilog;
using Constants = Mosaico.Domain.BusinessManagement.Constants;

namespace Mosaico.Application.BusinessManagement.Commands.UpdateInvitation
{
    public class UpdateTeamMemberCommandHandler : IRequestHandler<UpdateTeamMemberCommand, Guid>
    {
        private readonly IBusinessDbContext _context;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILogger _logger;


        public UpdateTeamMemberCommandHandler(IBusinessDbContext dbContext, IEventFactory eventFactory,
            IEventPublisher eventPublisher, ILogger logger = null)
        {
            _context = dbContext;
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _logger = logger;
        }

        public async Task<Guid> Handle(UpdateTeamMemberCommand request, CancellationToken cancellationToken)
        {
            using (var transaction = _context.BeginTransaction())
            {
                try
                {
                    var invitation =
                        await _context.TeamMembers.FirstOrDefaultAsync(x => x.Id == request.Id && x.CompanyId == request.CompanyId,
                            cancellationToken);
                    if (invitation != null)
                    {
                        var role = _context.TeamMemberRoles
                            .FirstOrDefaultAsync(s => s.Key == request.RoleName, cancellationToken).Result;
                        var ownerCount = await _context.TeamMembers.CountAsync(x => x.CompanyId == invitation.CompanyId && x.IsAccepted &&
                            x.TeamMemberRole.Key == Domain.BusinessManagement.Constants.TeamMemberRoles.Owner, cancellationToken: cancellationToken);
                        if (ownerCount <= 1 && invitation.TeamMemberRole.Key == Domain.BusinessManagement.Constants.TeamMemberRoles.Owner &&
                            invitation.IsAccepted) throw new NotAllowedToRemoveTheOnlyOwnerException();
                        invitation.TeamMemberRoleId = role.Id;

                        await _context.SaveChangesAsync(cancellationToken);
                        await transaction.CommitAsync(cancellationToken);
                        _logger?.Verbose("Company invitation successfully updated");
                        await PublishEvents(invitation.CompanyId, invitation.Id);

                        return invitation.Id;
                    }

                    throw new InvitationNotFoundException(request.Id);
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