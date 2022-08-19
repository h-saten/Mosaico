using System;
using System.Text.RegularExpressions;
using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Commands.AddProjectMember
{
    public class AddProjectMemberCommandValidator : AbstractValidator<AddProjectMemberCommand>
    {
        public AddProjectMemberCommandValidator()
        {
            RuleFor(m => m.Email).NotEmpty().Matches(c => new Regex("^([a-z0-9]+(?:[._-][a-z0-9]+)*)@([a-z0-9]+(?:[.-][a-z0-9]+)*\\.[a-z]{2,})$"));
            RuleFor(m => m.ProjectId).NotNull().Must(i => i != Guid.Empty);
            RuleFor(m => m.Role).NotNull();
        }
    }
}