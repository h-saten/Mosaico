using FluentValidation;

namespace Mosaico.Application.Wallet.Commands.Staking.WithdrawMetamask
{
    public class WithdrawMetamaskCommandValidator : AbstractValidator<WithdrawMetamaskCommand>
    {
        public WithdrawMetamaskCommandValidator()
        {
            RuleFor(t => t.Id).NotEmpty();
            RuleFor(t => t.UserId).NotEmpty();
        }
    }
}