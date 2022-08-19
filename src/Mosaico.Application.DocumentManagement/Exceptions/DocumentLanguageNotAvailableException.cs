using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaico.Application.DocumentManagement.Exceptions
{
    public class DocumentLanguageNotAvailableException : ExceptionBase
    {
        public DocumentLanguageNotAvailableException(string language) : base($"Docuemnt doesn't contain any content in {language}")
        {
        }

        public override string Code => Constants.ErrorCodes.DocumentLanguageNotAvailable;

        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}
