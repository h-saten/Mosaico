using System;
using FluentValidation;

namespace Mosaico.Application.Wallet.Queries.WalletTokens
{
    public class WalletTokensQueryValidator : AbstractValidator<WalletTokensQuery>
    {
        public WalletTokensQueryValidator()
        {
            RuleFor(q => q.UserId).NotEmpty().Must(t => Guid.TryParse(t, out var userId));
            RuleFor(d => d.Network)
                .Must(c => Blockchain.Base.Constants.BlockchainNetworks.All.Contains(c))
                .WithMessage($"Chain not supported");
        }
    }
}