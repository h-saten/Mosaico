using FluentValidation;

namespace Mosaico.Application.Wallet.Commands.TokenManagement.EnableStaking
{
    public class EnableStakingCommandValidator : AbstractValidator<EnableStakingCommand>
    {
        public EnableStakingCommandValidator()
        {
            RuleFor(t => t.TokenId).NotEmpty();
        }
    }
}