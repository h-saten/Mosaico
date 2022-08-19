using System;
using MediatR;

namespace Mosaico.Application.Wallet.Commands.NFT.CreateNFTCollection
{
    public class CreateNFTCollectionCommand : IRequest<Guid>
    {
        public string Name { get; set; }
        public string Symbol { get; set; }
        public string Network { get; set; }
        public Guid? CompanyId { get; set; }
        public Guid? ProjectId { get; set; }
        public string Address { get; set; }
    }
}