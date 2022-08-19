using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Authorization.Base;
using Mosaico.Domain.BusinessManagement.Abstractions;
using Mosaico.Domain.BusinessManagement.Entities;
using Mosaico.Domain.BusinessManagement.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.BusinessManagement;
using Serilog;
using Mosaico.Domain.BusinessManagement;
using Constants = Mosaico.Domain.BusinessManagement.Constants;

namespace Mosaico.Application.BusinessManagement.Commands.LeaveCompany
{
    public class LeaveCompanyCommandHandler : IRequestHandler<LeaveCompanyCommand>
    {
        private readonly IBusinessDbContext _context;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly ICurrentUserContext _currentUserContext;

        public LeaveCompanyCommandHandler(IBusinessDbContext context, IEventFactory eventFactory, IEventPublisher eventPublisher, IMapper mapper, ICurrentUserContext currentUserContext, ILogger logger = null)
        {
            _context = context;
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _mapper = mapper;
            _logger = logger;
            _currentUserContext = currentUserContext;
        }

        public async Task<Unit> Handle(LeaveCompanyCommand request, CancellationToken cancellationToken)
        {
            using (var transaction = _context.BeginTransaction())
            {
                try
                {
                    _logger?.Verbose($"Attempting to leave the company");
                    var userId = _currentUserContext.UserId;
                    var companyInvitation = await _context.TeamMembers.FirstOrDefaultAsync(p => p.CompanyId == request.CompanyId && p.UserId == userId, cancellationToken);
                    if (companyInvitation == null)
                    {
                        throw new InvitationNotFoundException(request.CompanyId);
                    }
                    var ownerCount = await _context.TeamMembers.CountAsync(x => x.CompanyId == companyInvitation.CompanyId && x.IsAccepted && x.TeamMemberRole.Key == Domain.BusinessManagement.Constants.TeamMemberRoles.Owner, cancellationToken: cancellationToken);
                    if (ownerCount <= 1)
                    {
                        throw new NotAllowedToRemoveTheOnlyOwnerException();
                    }
                    _logger?.Verbose($"Company invitation was found");
                    _context.TeamMembers.Remove(companyInvitation);
                    await _context.SaveChangesAsync(cancellationToken);
                    _logger?.Verbose($"Successfully left the company");
                    _logger?.Verbose($"Attempting to send events");
                    await PublishEventAsync(companyInvitation.CompanyId, companyInvitation.UserId, companyInvitation.Email);
                    _logger?.Verbose($"Events were sent");
                    await transaction.CommitAsync(cancellationToken);
                    return Unit.Value;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }
        }

        private async Task PublishEventAsync(Guid companyId, string userId, string email)
        {
            var e = _eventFactory.CreateEvent(Events.BusinessManagement.Constants.EventPaths.Companies,
                new CompanyInvitationDeletedEvent(companyId, email, userId));
            await _eventPublisher.PublishAsync(Events.BusinessManagement.Constants.EventPaths.Companies, e);
        }
    }
}