using System;

namespace Mosaico.Application.Wallet.DTO
{
    public class TokenDistributionDTO
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public decimal TokenAmount { get; set; }
        public decimal? TokenPrice { get; set; }
        public Guid? ProjectId { get; set; }
        public bool Blocked { get; set; }
        public decimal? Balance { get; set; }
    }
}