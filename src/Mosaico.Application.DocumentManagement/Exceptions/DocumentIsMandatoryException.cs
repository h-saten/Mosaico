using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaico.Application.DocumentManagement.Exceptions
{
    public class DocumentIsMandatoryException : ExceptionBase
    {
        public DocumentIsMandatoryException(string documentId) : base($"Can't modify mandatory document {documentId}")
        {
        }

        public override string Code => Constants.ErrorCodes.DocumentIsMandatory;

        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}
