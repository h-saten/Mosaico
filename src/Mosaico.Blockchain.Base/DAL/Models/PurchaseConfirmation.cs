using System;
using System.Numerics;

namespace Mosaico.Blockchain.Base.DAL.Models
{
    public class PurchaseConfirmation
    {
        public string Payer { get; set; }
        public string Beneficiary { get; set; }
        public string PaymentCurrencyAddress { get; set; }
        public BigInteger PayedAmount { get; set; }
        public BigInteger ReceivedTokensAmount { get; set; }
        public DateTimeOffset Date { get; set; }
    }
}