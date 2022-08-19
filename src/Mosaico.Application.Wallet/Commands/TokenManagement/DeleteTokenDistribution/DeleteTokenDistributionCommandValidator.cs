using FluentValidation;

namespace Mosaico.Application.Wallet.Commands.TokenManagement.DeleteTokenDistribution
{
    public class DeleteTokenDistributionCommandValidator : AbstractValidator<DeleteTokenDistributionCommand>
    {
        public DeleteTokenDistributionCommandValidator()
        {
            RuleFor(t => t.TokenId).NotEmpty();
            RuleFor(t => t.TokenDistributionId).NotEmpty();
        }
    }
}