using FluentValidation;

namespace Mosaico.Application.Wallet.Queries.GetPaymentCurrencyBalance
{
    public class GetPaymentCurrencyBalanceQueryValidator : AbstractValidator<GetPaymentCurrencyBalanceQuery>
    {
        public GetPaymentCurrencyBalanceQueryValidator()
        {
            RuleFor(t => t.Network).NotEmpty();
            RuleFor(t => t.UserId).NotEmpty();
        }
    }
}