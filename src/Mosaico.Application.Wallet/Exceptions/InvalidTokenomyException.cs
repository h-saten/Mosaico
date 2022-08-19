using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.Wallet.Exceptions
{
    public class InvalidTokenomyException: ExceptionBase

    {
        public InvalidTokenomyException(string message, string code) : base(message)
        {
            Code = code;
        }

        public override string Code { get; }
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}