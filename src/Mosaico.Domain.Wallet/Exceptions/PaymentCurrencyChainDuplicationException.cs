using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.Wallet.Exceptions
{
    public class PaymentCurrencyChainDuplicationException : ExceptionBase
    {
        public PaymentCurrencyChainDuplicationException(string chain) : base($"Currency on chain '{chain}' already exist.")
        {
            
        }
        
        public override string Code => Constants.ErrorCodes.CurrencyNetworkDuplication;
        public override int StatusCode => StatusCodes.Status403Forbidden;
    }
}