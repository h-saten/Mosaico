using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.Wallet.Exceptions
{
    public class TokenDistributionAlreadyExistsException : ExceptionBase
    {
        public TokenDistributionAlreadyExistsException(string name) : base($"Token distribution with name {name} already exists")
        {
        }

        public override string Code => Constants.ErrorCodes.TokenDistributionAlreadyExists;
        public override int StatusCode => StatusCodes.Status409Conflict;
    }
}