using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.Wallet.Exceptions
{
    public class TokenDistributionNotFoundException : ExceptionBase
    {
        public TokenDistributionNotFoundException(string name) : base($"Token distribution with name {name} already found")
        {
        }

        public override string Code => Constants.ErrorCodes.TokenDistributionNotFound;
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}