using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.BusinessManagement.Exceptions
{
    public class SubscriptionAlreadyExistsException : ExceptionBase
    {
        public SubscriptionAlreadyExistsException(string message) : base($"Subscription {message} not found")
        {
        }

        public override string Code => Constants.ErrorCodes.SubscriptionAlreadyExists;
        public override int StatusCode => StatusCodes.Status409Conflict;
    }
}