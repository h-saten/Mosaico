namespace Mosaico.Payments.Transak
{
    public static class Constants
    {
        public const string PaymentProcessorName = "TRANSAK";
        public static class TransactionStatuses
        {
            public const string PROCESSING = "PROCESSING";
            public const string CANCELLED = "CANCELLED";
            public const string FAILED = "FAILED";
            public const string SUCCEEDED = "COMPLETED";
            public const string EXPIRED = "EXPIRED";
        }
    }
}