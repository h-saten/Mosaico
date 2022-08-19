using System;
using System.Collections.Generic;
using Mosaico.Domain.Wallet.Entities.Enums;

namespace Mosaico.SDK.Wallet.Models
{
    public class MosaicoToken
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public string Address { get; set; }
        public string OwnerAddress { get; set; }
        public decimal TotalSupply { get; set; }
        public string Network { get; set; }
        public TokenStatus Status { get; set; }
        public decimal TokensLeft { get; set; }
        public bool IsMintable { get; set; }
        public bool IsBurnable { get; set; }
        public bool HasStaking { get; set; }
        public string Type { get; set; }
        public string LogoUrl { get; set; }
        public List<TokenExchange> Exchanges { get; set; }
    }
}