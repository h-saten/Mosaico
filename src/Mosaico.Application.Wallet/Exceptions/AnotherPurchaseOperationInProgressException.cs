using System;
using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.Wallet.Exceptions
{
    public class AnotherPurchaseOperationInProgressException : ExceptionBase
    {
        public AnotherPurchaseOperationInProgressException(string userId, Guid projectId) : base($"User {userId} has another purchase operation in progress for project {projectId}")
        {
        }

        public override string Code => "ANOTHER_PURCHASE_IN_PROGRESS";
        public override int StatusCode => StatusCodes.Status409Conflict;
    }
}