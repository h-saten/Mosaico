using System;

namespace Mosaico.SDK.Wallet.Models
{
    public class MosaicoTokenDistribution
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal TokenAmount { get; set; }
    }
}