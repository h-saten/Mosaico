using FluentValidation;
using Mosaico.Application.Wallet.DTO;

namespace Mosaico.Application.Wallet.Validators
{
    public class CreateVestingInvitationValidator : AbstractValidator<CreateVestingInvitationDTO>
    {
        public CreateVestingInvitationValidator()
        {
            RuleFor(t => t).Must(t => !string.IsNullOrWhiteSpace(t.Email) || !string.IsNullOrWhiteSpace(t.Phone) || !string.IsNullOrWhiteSpace(t.WalletAddress));
        }
    }
}