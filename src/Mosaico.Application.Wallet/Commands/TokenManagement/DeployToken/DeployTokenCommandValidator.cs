using FluentValidation;

namespace Mosaico.Application.Wallet.Commands.TokenManagement.DeployToken
{
    public class DeployTokenCommandValidator : AbstractValidator<DeployTokenCommand>
    {
        public DeployTokenCommandValidator()
        {
            RuleFor(t => t.TokenId).NotEmpty();
        }
    }
}