using FluentValidation;
using Mosaico.Domain.BusinessManagement.Abstractions;

namespace Mosaico.Application.BusinessManagement.Commands.CreateCompanyTeamMember
{
    public class CreateTeamMemberCommandValidator : AbstractValidator<CreateTeamMemberCommand>
    {
        public CreateTeamMemberCommandValidator(IBusinessDbContext context)
        {
            RuleSet("default", () =>
            {
                RuleFor(e => e.Role).NotEmpty();
            });
        }
    }
}
