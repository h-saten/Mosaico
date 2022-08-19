using System;

namespace Mosaico.Application.Wallet.DTO
{
    public class NFTDTO
    {
        public string TokenId { get; set; }
        public string Uri { get; set; }
        public string OwnerAddress { get; set; }
        public Guid NFTCollectionId { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public string Network { get; set; }
    }
}