using System;

namespace Mosaico.Application.Wallet.DTO
{
    public class TokenExternalExchangeDTO
    {
        public ExternalExchangeDTO ExternalExchange { get; set; }
        public DateTimeOffset? ListedAt { get; set; }
    }
}