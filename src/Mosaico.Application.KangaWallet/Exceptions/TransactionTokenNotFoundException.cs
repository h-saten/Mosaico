using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.KangaWallet.Exceptions
{
    public class TransactionTokenNotFoundException : ExceptionBase
    {
        public string Token { get; set; }
        
        public TransactionTokenNotFoundException(string tokenSymbol) : base($"Token {tokenSymbol} not found.")
        {
            Token = tokenSymbol;
        }

        public override string Code => KangaExchange.SDK.Constants.ErrorCodes.TokenNotFound;
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}