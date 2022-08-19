using FluentValidation;

namespace Mosaico.Application.Wallet.Commands.NFT.CreateNFTCollection
{
    public class CreateNFTCollectionCommandValidator : AbstractValidator<CreateNFTCollectionCommand>
    {
        public CreateNFTCollectionCommandValidator()
        {
            RuleFor(t => t.Network).Must(n => Blockchain.Base.Constants.BlockchainNetworks.All.Contains(n));
            RuleFor(t => t.Name).Length(3, 50);
            RuleFor(t => t.Symbol).Length(3, 20);
        }
    }
}