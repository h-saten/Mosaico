using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.ProjectManagement.Exceptions
{
    public class SubscriptionNotFoundException : ExceptionBase
    {
        public SubscriptionNotFoundException(string email) : base($"Subscription {email} was not found")
        {
        }

        public override string Code => Constants.ErrorCodes.SubscriptionNotFound;
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}