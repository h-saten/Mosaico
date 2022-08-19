using System;
using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.Wallet.Exceptions
{
    public class OtherOperationInProgressException : ExceptionBase
    {
        public OtherOperationInProgressException(Guid transactionId) : base($"Transaction {transactionId} has another operation in progress")
        {
        }

        public override string Code => "OTHER_OPERATION_IN_PROGRESS";
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}