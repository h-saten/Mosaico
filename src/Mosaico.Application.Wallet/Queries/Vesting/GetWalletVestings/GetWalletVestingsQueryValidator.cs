using FluentValidation;

namespace Mosaico.Application.Wallet.Queries.Vesting.GetWalletVestings
{
    public class GetWalletVestingsQueryValidator : AbstractValidator<GetWalletVestingsQuery>
    {
        public GetWalletVestingsQueryValidator()
        {
            RuleFor(t => t.UserId).NotEmpty();
            RuleFor(t => t.Network).Must(c => Blockchain.Base.Constants.BlockchainNetworks.All.Contains(c));
        }
    }
}