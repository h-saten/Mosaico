using FluentValidation;

namespace Mosaico.Application.Wallet.Queries.GetVesting
{
    public class GetVestingQueryValidator : AbstractValidator<GetVestingQuery>
    {
        public GetVestingQueryValidator()
        {
            RuleFor(t => t.TokenId).NotEmpty();
        }
    }
}