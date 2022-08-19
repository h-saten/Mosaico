using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.BusinessManagement.Abstractions;
using Mosaico.Domain.BusinessManagement.Entities;
using Mosaico.Domain.BusinessManagement.Exceptions;
using Mosaico.Integration.Email.Abstraction;
using Serilog;
using Constants = Mosaico.Domain.BusinessManagement.Constants;

namespace Mosaico.Application.BusinessManagement.Commands.ResendInvitation
{
    public class ResendInvitationCommandHandler : IRequestHandler<ResendInvitationCommand>
    {
        private readonly IBusinessDbContext _context;
        private readonly ILogger _logger;
        private readonly IEmailSender _emailSender;


        public ResendInvitationCommandHandler(IBusinessDbContext dbContext, IEmailSender emailSender,  ILogger logger = null)
        {
            _context = dbContext;
            _logger = logger;
            _emailSender = emailSender;
        }
        public async Task<Unit> Handle(ResendInvitationCommand request, CancellationToken cancellationToken)
        {

            using (var transaction = _context.BeginTransaction())
            {
                try
                {
                    var invitation = await _context.TeamMembers.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
                    if (invitation != null)
                    {
                        invitation.ExpiresAt = DateTimeOffset.UtcNow.AddDays(Domain.BusinessManagement.Constants.Invitation.AcceptanceThreshold);
                        _context.TeamMembers.Update(invitation);
                        await _context.SaveChangesAsync(cancellationToken);
                        _logger?.Verbose($"Company invitation was successfully updated");
                        await transaction.CommitAsync(cancellationToken);
                        _logger?.Verbose($"Company invitation resent successfully");
                        await SendInvitationToEmail(invitation);

                        return Unit.Value;
                    }
                    else
                        throw new InvitationNotFoundException(request.Id);
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }
        }

        //TODO: wft. refactor
        private async Task SendInvitationToEmail(TeamMember companyInvitation)
        {
            var email = new Email
            {
                Html = "Visit the following URL to accept the company invitation http://localhost:4200/api/companies/" + companyInvitation.Id + "/acceptinvitation",
                Recipients = new List<string> { companyInvitation.Email },
                Subject = "Accept company invitation"
            };

            await _emailSender.SendAsync(email);
        }
    }
}
