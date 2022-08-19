using System;
using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.Core.Exceptions
{
    public class ContactMessageException : ExceptionBase
    {
        public ContactMessageException() : base($"Cannot send message.")
        {
        }

        public override string Code => Constants.ErrorCodes.MessageSendingError;
        public override int StatusCode => StatusCodes.Status404NotFound;    
    }
}
