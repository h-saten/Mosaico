using FluentValidation;
using Mosaico.Application.ProjectManagement.Validators;
using Mosaico.Application.Wallet.DTO;

namespace Mosaico.Application.Wallet.Validators
{
    public class CreateVestingFundValidator : AbstractValidator<CreateVestingFundDTO>
    {
        public CreateVestingFundValidator()
        {
            RuleFor(f => f.Days).GreaterThan(0);
            RuleFor(f => f.Distribution).GreaterThan(0);
            RuleFor(f => f.Name).NotEmpty().NotNull();
            RuleFor(f => f.Invitations).Must(i => i != null && i.Count > 0);
            RuleForEach(f => f.Invitations).SetValidator(new CreateVestingInvitationValidator());
            RuleFor(f => f.SubtractedPercent).GreaterThan(0).LessThanOrEqualTo(100)
                .When(f => f.SubtractedPercent.HasValue);
        }
    }
}