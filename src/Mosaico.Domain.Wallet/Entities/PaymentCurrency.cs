using Mosaico.Domain.Base;

namespace Mosaico.Domain.Wallet.Entities
{
    public class PaymentCurrency : EntityBase
    {
        public string Name { get; set; }
        public string Ticker { get; set; }
        public string LogoUrl { get; set; }
        public bool NativeChainCurrency { get; set; }
        public string ContractAddress { get; set; }
        public string Chain { get; set; }
        public int DecimalPlaces { get; set; }
        
        public static PaymentCurrency AddNativeCurrency(string name, string ticker, string chain)
        {
            var entity = new PaymentCurrency
            {
                Name = name,
                Ticker = ticker,
                Chain = chain,
                NativeChainCurrency = true,
                DecimalPlaces = 18
            };

            return entity;
        }
        
        public static PaymentCurrency AddStableCoin(string name, string ticker, string chain, string contractAddress, int decimalPlaces = 6)
        {
            var entity = new PaymentCurrency
            {
                Name = name,
                Ticker = ticker,
                Chain = chain,
                NativeChainCurrency = false,
                DecimalPlaces = decimalPlaces,
                ContractAddress = contractAddress
            };

            return entity;
        }
    }
}