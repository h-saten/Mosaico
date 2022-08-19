using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.KangaWallet.Exceptions
{
    public class KangaBuyFailedException : ExceptionBase
    {
        public decimal Amount { get; set; }
        public string TokenSymbol { get; set; }
        public string Email { get; set; }
        
        public KangaBuyFailedException(decimal amount, string tokenSymbol, string email) : base($"Transaction of {amount}{tokenSymbol} for email: '{email}' failed.")
        {
            Amount = amount;
            TokenSymbol = tokenSymbol;
            Email = email;
        }

        public override string Code => KangaExchange.SDK.Constants.ErrorCodes.KangaException;
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}