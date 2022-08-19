using FluentValidation;

namespace Mosaico.Application.Wallet.Queries.GetHistoricalExchangeRate
{
    public class GetHistoricalExchangeRateQueryValidator : AbstractValidator<GetHistoricalExchangeRateQuery>
    {
        public GetHistoricalExchangeRateQueryValidator()
        {
            RuleFor(t => t.Symbol).NotEmpty();
        }
    }
}