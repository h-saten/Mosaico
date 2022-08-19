using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.Features.Exceptions
{
    public class BetaTesterNotFoundException : ExceptionBase
    {
        public BetaTesterNotFoundException(string id) : base($"Beta tester {id} not found")
        {
        }

        public override string Code => Constants.ErrorCodes.BetaTesterNotFound;
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}