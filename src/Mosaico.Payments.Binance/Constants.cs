using System.Collections.Generic;

namespace Mosaico.Payments.Binance
{
    public static class Constants
    {
        public const string PaymentProcessorName = "BINANCE";
        
        public static class TransactionStatuses
        {
            public const string INITIAL = "INITIAL";
            public const string PROCESSING = "PENDING";
            public const string CANCELLED = "CANCELED";
            public const string FAILED = "ERROR";
            public const string SUCCEEDED = "PAID";
            public const string EXPIRED = "EXPIRED";
        }

        public static class Currencies
        {
            public static readonly List<string> All = new List<string> { USDT, BUSD };
            
            public const string USDT = "USDT";
            public const string BUSD = "BUSD";
        }
    }
}