using System;

namespace Mosaico.Domain.Wallet.Entities
{
    public class TokenToExternalExchange
    {
        public Guid TokenId { get; set; }
        public Guid ExternalExchangeId { get; set; }
        public virtual Token Token { get; set; }
        public virtual ExternalExchange ExternalExchange { get; set; }
        public DateTimeOffset? ListedAt { get; set; }
    }
}