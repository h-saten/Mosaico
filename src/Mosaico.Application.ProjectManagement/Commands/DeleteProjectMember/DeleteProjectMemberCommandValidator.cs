using System;
using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Commands.DeleteProjectMember
{
    public class DeleteProjectMemberCommandValidator : AbstractValidator<DeleteProjectMemberCommand>
    {
        public DeleteProjectMemberCommandValidator()
        {
            RuleFor(c => c.ProjectMemberId).NotEmpty().Must(c => c != Guid.Empty);
            RuleFor(c => c.ProjectId).NotEmpty().Must(c => c != Guid.Empty);
        }
    }
}