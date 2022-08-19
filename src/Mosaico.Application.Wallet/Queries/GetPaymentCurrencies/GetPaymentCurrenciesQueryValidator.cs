using FluentValidation;

namespace Mosaico.Application.Wallet.Queries.GetPaymentCurrencies
{
    public class GetPaymentCurrenciesQueryValidator : AbstractValidator<GetPaymentCurrenciesQuery>
    {
        public GetPaymentCurrenciesQueryValidator()
        {
            RuleFor(t => t.Network).Must(t => Blockchain.Base.Constants.BlockchainNetworks.All.Contains(t));
        }
    }
}