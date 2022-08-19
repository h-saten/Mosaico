using FluentValidation;

namespace Mosaico.Application.Wallet.Commands.TokenManagement.EnableVesting
{
    public class EnableVestingCommandValidator : AbstractValidator<EnableVestingCommand>
    {
        public EnableVestingCommandValidator()
        {
            RuleFor(t => t.TokenId).NotEmpty();
        }
    }
}