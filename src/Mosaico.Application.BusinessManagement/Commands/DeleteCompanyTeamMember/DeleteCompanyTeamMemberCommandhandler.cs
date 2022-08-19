using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.BusinessManagement.Abstractions;
using Mosaico.Domain.BusinessManagement.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.BusinessManagement;
using Serilog;

namespace Mosaico.Application.BusinessManagement.Commands.DeleteCompanyTeamMember
{
    public class DeleteCompanyTeamMemberCommandHandler : IRequestHandler<DeleteCompanyTeamMemberCommand>
    {
        private readonly IBusinessDbContext _context;
        private readonly ILogger _logger;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;

        public DeleteCompanyTeamMemberCommandHandler(IBusinessDbContext dbContext, IEventFactory eventFactory, IEventPublisher eventPublisher, ILogger logger = null)
        {
            _context = dbContext;
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _logger = logger;
        }
        public async Task<Unit> Handle(DeleteCompanyTeamMemberCommand request, CancellationToken cancellationToken)
        {
            using (var transaction = _context.BeginTransaction())
            {
                try
                {
                    var teamMember = await _context.TeamMembers.FirstOrDefaultAsync(x => x.Id == request.Id && x.CompanyId == request.CompanyId, cancellationToken: cancellationToken);
                    if (teamMember != null)
                    {
                        var ownerCount = await _context.TeamMembers
                            .CountAsync(x => x.CompanyId == teamMember.CompanyId && x.IsAccepted
                                                                              && x.TeamMemberRole.Key == Domain.BusinessManagement.Constants.TeamMemberRoles.Owner, 
                                cancellationToken: cancellationToken);
                        
                        if (ownerCount <= 1 && teamMember.TeamMemberRole.Key == Domain.BusinessManagement.Constants.TeamMemberRoles.Owner && teamMember.IsAccepted)
                        {
                            throw new NotAllowedToRemoveTheOnlyOwnerException();
                        }
                        
                        _context.TeamMembers.Remove(teamMember);
                        await _context.SaveChangesAsync(cancellationToken);
                        await transaction.CommitAsync(cancellationToken);
                        _logger?.Verbose($"Company team member removed successfully");
                        await PublishEventAsync(teamMember.CompanyId, teamMember.UserId, teamMember.Email);
                        return Unit.Value;
                    }
                    throw new TeamMemberNotFoundException(request.Id);
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
            await _eventPublisher.PublishAsync(e);
        }
    }
}
