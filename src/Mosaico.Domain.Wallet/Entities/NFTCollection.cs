using System;
using System.Collections.Generic;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.Wallet.Entities
{
    public class NFTCollection : EntityBase
    {
        public string Name { get; set; }
        public string Symbol { get; set; }
        public string Network { get; set; }
        public Guid? CompanyId { get; set; }
        public Guid? ProjectId { get; set; }
        public string UserId { get; set; }
        public string Address { get; set; }
        public virtual List<NFT> NFTs { get; set; } = new List<NFT>();
    }
}