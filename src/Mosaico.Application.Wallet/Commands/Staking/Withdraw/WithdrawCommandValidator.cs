using FluentValidation;

namespace Mosaico.Application.Wallet.Commands.Staking.Withdraw
{
    public class WithdrawCommandValidator : AbstractValidator<WithdrawCommand>
    {
        public WithdrawCommandValidator()
        {
            RuleFor(t => t.Id).NotEmpty();
            RuleFor(t => t.UserId).NotEmpty();
        }
    }
}