using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.BusinessManagement.Abstractions;
using Mosaico.Domain.BusinessManagement.Entities;
using Mosaico.Domain.BusinessManagement.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.BusinessManagement;
using Mosaico.Integration.Email.Abstraction;
using Serilog;

namespace Mosaico.Application.BusinessManagement.Commands.CreateCompanyTeamMember
{
    public class CreateTeamMemberCommandHandler : IRequestHandler<CreateTeamMemberCommand, Guid>
    {
        private readonly IBusinessDbContext _context;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public CreateTeamMemberCommandHandler(IBusinessDbContext dbContext, IEventFactory eventFactory,
            IEventPublisher eventPublisher, IMapper mapper, IEmailSender emailSender, ILogger logger = null)
        {
            _context = dbContext;
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _mapper = mapper;
            _emailSender = emailSender;
            _logger = logger;
        }

        public async Task<Guid> Handle(CreateTeamMemberCommand request, CancellationToken cancellationToken)
        {
            using (var transaction = _context.BeginTransaction())
            {
                try
                {
                    var teamMember = _mapper.Map<TeamMember>(request);
                    var role = await _context.TeamMemberRoles.FirstOrDefaultAsync(r => r.Key == request.Role, cancellationToken);
                    if (role == null)
                    {
                        throw new CompanyRoleNotFoundException(request.Role);
                    }

                    teamMember.TeamMemberRole = role;
                    teamMember.TeamMemberRoleId = role.Id;

                    var invitationExists = await _context.TeamMembers.CountAsync(
                        t => t.Email == request.Email && t.CompanyId == request.CompanyId, cancellationToken);
                    
                    if (invitationExists > 0)
                    {
                        throw new InvitationAlreadyExistsException(request.Email);
                    }
                    
                    teamMember.AuthorizationCode = Guid.NewGuid().ToString();
                    teamMember.ExpiresAt = DateTimeOffset.UtcNow.AddDays(Domain.BusinessManagement.Constants.Invitation.AcceptanceThreshold);
                    
                    _context.TeamMembers.Add(teamMember);
                    await _context.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
                    await PublishEventAsync(request.CompanyId, request.Email);
                    await SendInvitationToEmail(teamMember);
                    _logger?.Verbose("Company team member successfully added");
                    return teamMember.Id;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }
        }
        
        //TODO: to the normal email
        private async Task SendInvitationToEmail(TeamMember companyInvitation)
        {
            var email = new Email
            {
                Html = "Visit the following URL to accept the company invitation http://localhost:4200/companies/invitation/" + companyInvitation.AuthorizationCode,
                Recipients = new List<string> { companyInvitation.Email },
                Subject = "Accept company invitation"
            };

            await _emailSender.SendAsync(email);
        }
        
        private async Task PublishEventAsync(Guid companyId, string email)
        {
            var e = _eventFactory.CreateEvent(Events.BusinessManagement.Constants.EventPaths.Companies, new CompanyInvitationAddedEvent(companyId, email));
            await _eventPublisher.PublishAsync(e);
        }
    }
}