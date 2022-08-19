using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaico.API.v1.DocumentManagement.Exceptions
{
    public class EmptyDocumentException : ExceptionBase
    {
        public EmptyDocumentException(): base("Document is empty")
        {

        }
        public override string Code => Constants.ErrorCodes.EmptyDocument;
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}
