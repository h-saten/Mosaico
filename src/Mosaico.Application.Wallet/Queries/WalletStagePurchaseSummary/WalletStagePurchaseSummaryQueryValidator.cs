using FluentValidation;

namespace Mosaico.Application.Wallet.Queries.WalletStagePurchaseSummary
{
    public class WalletStagePurchaseSummaryQueryValidator : AbstractValidator<WalletStagePurchaseSummaryQuery>
    {
        public WalletStagePurchaseSummaryQueryValidator()
        {
            RuleFor(q => q.UserId).NotEmpty();
            RuleFor(d => d.Network)
                .Must(c => Blockchain.Base.Constants.BlockchainNetworks.All.Contains(c))
                .WithMessage($"Chain not supported");
            RuleFor(d => d.StageId).NotEmpty();
        }
    }
}