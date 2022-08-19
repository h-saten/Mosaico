using FluentValidation;

namespace Mosaico.Application.Wallet.Queries.GetCompanyPaymentDetails
{
    public class GetCompanyPaymentDetailsQueryValidator : AbstractValidator<GetCompanyPaymentDetailsQuery>
    {
        public GetCompanyPaymentDetailsQueryValidator()
        {
            RuleFor(t => t.Currency).NotEmpty();
            RuleFor(t => t.CompanyId).NotEmpty();
        }
    }
}