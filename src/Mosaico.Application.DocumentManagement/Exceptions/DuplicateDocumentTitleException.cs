using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaico.Application.DocumentManagement.Exceptions
{
    public class DuplicateDocumentTitleException : ExceptionBase
    {
        public DuplicateDocumentTitleException(string documentName) : 
            base($"A document with title '${documentName}' already exists")
        {
        }

        public override string Code => Constants.ErrorCodes.DuplicateDocumentTitle;

        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}
