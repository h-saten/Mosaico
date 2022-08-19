using FluentValidation;

namespace Mosaico.Application.Wallet.Queries.WalletToken
{
    public class WalletTokenQueryValidator : AbstractValidator<WalletTokenQuery>
    {
        public WalletTokenQueryValidator()
        {
            RuleFor(q => q.UserId).NotEmpty();
            RuleFor(q => q.TokenId).NotEmpty();
        }
    }
}