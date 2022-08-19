using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaico.Domain.DocumentManagement.Exceptions
{
    public class DocumentNotFoundException : ExceptionBase
    {
        public DocumentNotFoundException(string id) : base($"Document {id} not found")
        {

        }
        public override string Code => Constants.ErrorCodes.DocumentNotFound;

        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}
