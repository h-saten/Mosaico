using FluentValidation;
using Mosaico.Domain.BusinessManagement.Abstractions;

namespace Mosaico.Application.BusinessManagement.Commands.UpdateInvitation
{
    public class UpdateTeamMemberCommandValidator : AbstractValidator<UpdateInvitation.UpdateTeamMemberCommand>
    {
        public UpdateTeamMemberCommandValidator(IBusinessDbContext context)
        {
            RuleSet("default", () =>
            {
                RuleFor(e => e.RoleName).NotEmpty();
            });
        }
    }
}
