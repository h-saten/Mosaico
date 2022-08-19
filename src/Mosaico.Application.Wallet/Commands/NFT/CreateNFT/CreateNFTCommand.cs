using System;
using MediatR;

namespace Mosaico.Application.Wallet.Commands.NFT.CreateNFT
{
    public class CreateNFTCommand : IRequest<Guid>
    {
        public Guid NFTCollectionId { get; set; }
        public string Uri { get; set; }
        public string TokenId { get; set; }
        public string OwnerAddress { get; set; }
    }
}