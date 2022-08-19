using FluentValidation;

namespace Mosaico.Application.Wallet.Commands.NFT.CreateNFT
{
    public class CreateNFTCommandValidator : AbstractValidator<CreateNFTCommand>
    {
        public CreateNFTCommandValidator()
        {
            RuleFor(t => t.Uri).NotEmpty();
            RuleFor(t => t.TokenId).NotEmpty();
            RuleFor(t => t.NFTCollectionId).NotEmpty();
        }
    }
}