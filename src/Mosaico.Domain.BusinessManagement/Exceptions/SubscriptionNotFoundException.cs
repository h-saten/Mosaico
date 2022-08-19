using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.BusinessManagement.Exceptions
{
    public class SubscriptionNotFoundException : ExceptionBase
    {
        public SubscriptionNotFoundException(string message) : base($"Subscription {message} was not found")
        {
        }

        public override string Code => Constants.ErrorCodes.SubscriptionNotFound;
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}