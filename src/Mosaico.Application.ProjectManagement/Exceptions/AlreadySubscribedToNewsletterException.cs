using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.ProjectManagement.Exceptions
{
    public class AlreadySubscribedToNewsletterException : ExceptionBase
    {
        public AlreadySubscribedToNewsletterException(string email) : base($"Email {email} already subscribed to the newsletter")
        {
        }

        public override string Code => Constants.ErrorCodes.AlreadySubscribedToNewsletter;
        public override int StatusCode => StatusCodes.Status409Conflict;
    }
}