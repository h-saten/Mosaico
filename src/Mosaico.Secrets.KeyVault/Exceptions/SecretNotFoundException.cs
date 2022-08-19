using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Secrets.KeyVault.Exceptions
{
    public class SecretNotFoundException : ExceptionBase
    {
        public SecretNotFoundException(string message) : base($"Secret {message} was not found")
        {
        }

        public override string Code => Constants.ErrorCodes.SecretNotFound;
        public override int StatusCode => StatusCodes.Status500InternalServerError;
    }
}