using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.Wallet.Exceptions
{
    public class InsufficientCurrencyBalanceException : ExceptionBase
    {
        public string Currency { get; set; }
        
        public InsufficientCurrencyBalanceException(string currency) : base($"User has insufficient {currency} balance.")
        {
            Currency = currency;
        }

        public override string Code => Constants.ErrorCodes.InvalidTransactionStatus;
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}