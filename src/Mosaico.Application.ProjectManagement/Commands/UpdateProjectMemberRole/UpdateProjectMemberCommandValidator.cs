using System;
using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Commands.UpdateProjectMemberRole
{
    public class UpdateProjectMemberCommandValidator : AbstractValidator<UpdateProjectMemberCommand>
    {
        public UpdateProjectMemberCommandValidator()
        {
            RuleFor(c => c.Role).NotNull().NotEmpty();
            RuleFor(c => c.MemberId).NotNull().Must(c => c != Guid.Empty);
            RuleFor(c => c.ProjectId).NotNull().Must(c => c != Guid.Empty);
        }
    }
}