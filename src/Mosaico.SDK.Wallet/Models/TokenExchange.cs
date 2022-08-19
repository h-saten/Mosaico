using System;

namespace Mosaico.SDK.Wallet.Models
{
    public class TokenExchange
    {
        public Guid Id { get; set; }
        public string LogoUrl { get; set; }
        public string Url { get; set; }
        public DateTimeOffset? ListedAt { get; set; }
        public bool IsDisabled { get; set; }
    }
}