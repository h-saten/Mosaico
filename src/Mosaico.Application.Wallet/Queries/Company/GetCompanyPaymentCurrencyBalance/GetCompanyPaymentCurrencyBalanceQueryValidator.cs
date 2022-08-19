using FluentValidation;

namespace Mosaico.Application.Wallet.Queries.Company.GetCompanyPaymentCurrencyBalance
{
    public class GetCompanyPaymentCurrencyBalanceQueryValidator : AbstractValidator<GetCompanyPaymentCurrencyBalanceQuery>
    {
        public GetCompanyPaymentCurrencyBalanceQueryValidator()
        {
            RuleFor(t => t.CompanyId).NotEmpty();
        }
    }
}