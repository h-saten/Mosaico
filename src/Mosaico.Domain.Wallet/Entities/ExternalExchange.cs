using System.Collections.Generic;
using Mosaico.Domain.Base;
using Mosaico.Domain.Wallet.Entities.Enums;

namespace Mosaico.Domain.Wallet.Entities
{
    public class ExternalExchange : EntityBase
    {
        public string Name { get; set; }
        public string LogoUrl { get; set; }
        public string Url { get; set; }
        public ExternalExchangeType Type { get; set; }
        public bool IsDisabled { get; set; }
        public virtual List<TokenToExternalExchange> Tokens { get; set; } = new List<TokenToExternalExchange>();
    }
}