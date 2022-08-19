using System;

namespace Mosaico.Application.Wallet.DTO
{
    public class VaultDTO
    {
        public Guid Id { get; set; }
        public string Address { get; set; }
        public Guid CompanyId { get; set; }
        public string Network { get; set; }
        public Guid TokenId { get; set; }
    }
}