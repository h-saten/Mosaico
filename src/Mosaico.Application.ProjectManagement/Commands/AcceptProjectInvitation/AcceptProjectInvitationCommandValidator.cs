using System;
using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Commands.AcceptProjectInvitation
{
    public class AcceptProjectInvitationCommandValidator : AbstractValidator<AcceptProjectInvitationCommand>
    {
        public AcceptProjectInvitationCommandValidator()
        {
            RuleFor(c => c.AuthorizationCode).NotEmpty();
        }
    }
}