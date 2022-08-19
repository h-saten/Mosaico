using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace KangaExchange.SDK
{
    public static class Constants
    {
        public static readonly ReadOnlyDictionary<string, string> KangaErrorCodes = new(
            new Dictionary<string, string>
            {
                {"9000", "Invalid signature"},
                {"9001", "No permissions for app"},
                {"9002", "Unknown token"}
            });

        public static class ErrorCodes
        {
            public const string KangaException = "KANGA_EXCEPTION";
            public const string UnsupportedCurrency = "UNSUPPORTED_CURRENCY";
            public const string UnsupportedPaymentMethod = "UNSUPPORTED_PAYMENT_METHOD";
            public const string TokenNotFound = "TOKEN_NOT_FOUND";
        }

        public static class KangaResponseStatuses
        {
            public const string OK = "ok";
        }

        public static class KangaAPIRoutes
        {
            public const string Profile = "v2/oauth2/profile";
            public const string Check = "v2/oauth2/check";
            public const string Markets = "markets";
        }

        public const string MarketProviderName = "KANGA_EXCHANGE";
        //TODO: To database
        public static readonly List<string> WhiteListedMarkets = new()
        {
            "MOS",
            "COP3",
            "COP2",
            "COP1",
            "6SD1",
            "GIN",
            "DANCE",
            "CBD",
            "CKWH",
            "MFT",
            "VIN21",
            "PAYB",
            "JPW21",
            "SPCY",
            "IFO",
            "JPW22",
            "MJC",
            "SCB",
            "APIS",
            "JT1",
            "HUB1",
            "VT1",
            "LAS",
            "CAN1"
        };

        public static readonly Dictionary<string, string> KangaMarketMapping = new()
        {
            {"RCMATIC", "RC"}
        };
        
        public static readonly Dictionary<string, string> KangaMarketMappingReverse = new()
        {
            {"RC", "RCMATIC"}
        };
    }
}