using FluentValidation;

namespace Mosaico.Application.Wallet.Queries.GetPaymentDetails
{
    public class GetPaymentDetailsQueryValidator : AbstractValidator<GetPaymentDetailsQuery>
    {
        public GetPaymentDetailsQueryValidator()
        {
            RuleFor(t => t.ProjectId);
        }
    }
}