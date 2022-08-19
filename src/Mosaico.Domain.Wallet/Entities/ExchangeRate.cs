using Mosaico.Domain.Base;

namespace Mosaico.Domain.Wallet.Entities
{
    public class ExchangeRate : EntityBase
    {
        public string Ticker { get; set; }
        public decimal Rate { get; set; }
        public string BaseCurrency { get; set; }
        public string Source { get; set; }
        public bool IsCrypto { get; set; }
    }
}