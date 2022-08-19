using FluentValidation;

namespace Mosaico.Application.Wallet.Queries.GetTokenDistribution
{
    public class GetTokenDistributionQueryValidator : AbstractValidator<GetTokenDistributionQuery>
    {
        public GetTokenDistributionQueryValidator()
        {
            RuleFor(t => t.TokenId).NotEmpty();
        }
    }
}