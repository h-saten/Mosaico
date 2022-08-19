using FluentValidation;

namespace Mosaico.Application.Wallet.Queries.WalletTransactions
{
    public class WalletTransactionsQueryValidator : AbstractValidator<WalletTransactionsQuery>
    {
        public WalletTransactionsQueryValidator()
        {
            RuleFor(q => q.WalletAddress).NotEmpty();
            RuleFor(d => d.Network)
                .Must(c => Blockchain.Base.Constants.BlockchainNetworks.All.Contains(c))
                .WithMessage($"Chain not supported");
            RuleFor(d => d.Skip).GreaterThanOrEqualTo(0);
            RuleFor(d => d.Take).GreaterThan(0);
        }
    }
}