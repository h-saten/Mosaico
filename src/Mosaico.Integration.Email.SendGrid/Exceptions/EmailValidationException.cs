using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaico.Integration.Email.SendGridEmail.Exceptions
{
    public class EmailValidationException : ExceptionBase
    {
        public EmailValidationException(string message) : base(message)
        {
        }

        public override string Code => Constants.ErrorCodes.EmailValidationError;
        public override int StatusCode => StatusCodes.Status500InternalServerError;
    }
}
