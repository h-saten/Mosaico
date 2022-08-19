using FluentValidation;

namespace Mosaico.Application.Wallet.Queries.GetToken
{
    public class GetTokenQueryValidator : AbstractValidator<GetTokenQuery>
    {
        public GetTokenQueryValidator()
        {
            RuleFor(t => t.Id).NotEmpty();
        }
    }
}