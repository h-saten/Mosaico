using System;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.Wallet.Entities
{
    public class NFT : EntityBase
    {
        public string TokenId { get; set; }
        public string Uri { get; set; }
        public string OwnerAddress { get; set; }
        public Guid NFTCollectionId { get; set; }
        public virtual NFTCollection NFTCollection { get; set; }
    }
}