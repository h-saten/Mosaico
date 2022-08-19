using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaico.Application.DocumentManagement.Exceptions
{
    public class DocumentLanguageAlreadyExistsException : ExceptionBase
    {
        public DocumentLanguageAlreadyExistsException(string language) : base($"Document in language {language} already exists")
        {
        }

        public override string Code => Constants.ErrorCodes.DocumentLanguageAlreadyExists;

        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}
