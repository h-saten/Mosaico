using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.Wallet.Exceptions
{
    public class OperationNotFoundException : ExceptionBase
    {
        public OperationNotFoundException(string id = null) : base($"Operation {id} not found")
        {
        }

        public override string Code => "OPERATION_NOT_FOUND";
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}