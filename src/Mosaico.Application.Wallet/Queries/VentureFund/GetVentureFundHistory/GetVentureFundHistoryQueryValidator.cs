using FluentValidation;

namespace Mosaico.Application.Wallet.Queries.VentureFund.GetVentureFundHistory
{
    public class GetVentureFundHistoryQueryValidator : AbstractValidator<GetVentureFundHistoryQuery>
    {
        public GetVentureFundHistoryQueryValidator()
        {
            RuleFor(t => t.Name).NotEmpty();
        }
    }
}