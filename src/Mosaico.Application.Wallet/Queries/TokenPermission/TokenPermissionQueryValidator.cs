using FluentValidation;

namespace Mosaico.Application.Wallet.Queries.TokenPermission
{
    public class TokenPermissionQueryValidator : AbstractValidator<TokenPermissionQuery>
    {
        public TokenPermissionQueryValidator()
        {
            RuleFor(t => t.TokenId).NotEmpty();
        }
    }
}