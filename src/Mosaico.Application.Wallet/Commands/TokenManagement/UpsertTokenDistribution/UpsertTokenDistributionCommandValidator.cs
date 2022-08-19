using FluentValidation;
using Mosaico.Application.Wallet.Validators;

namespace Mosaico.Application.Wallet.Commands.TokenManagement.UpsertTokenDistribution
{
    public class UpsertTokenDistributionCommandValidator : AbstractValidator<UpsertTokenDistributionCommand>
    {
        public UpsertTokenDistributionCommandValidator()
        {
            RuleFor(t => t.TokenId).NotEmpty();
            RuleFor(t => t.TokenDistributions).NotEmpty();
            RuleFor(t => t.TokenDistributions).ForEach(collection => collection.SetValidator(new TokenDistributionValidator()));
        }
    }
}