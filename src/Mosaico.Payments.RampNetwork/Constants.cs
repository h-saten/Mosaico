namespace Mosaico.Payments.RampNetwork
{
    public static class Constants
    {
        public const string PaymentProcessorName = "RAMP";
        public static class TransactionStatuses
        {
            public const string INITIALIZED = "INITIALIZED";
            public const string CANCELLED = "CANCELLED";
            public const string FAILED = "PAYMENT_FAILED";
            public const string SUCCEEDED = "RELEASED";
        }
    }
}