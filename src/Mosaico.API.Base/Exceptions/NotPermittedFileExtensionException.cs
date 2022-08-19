using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaico.API.Base.Exceptions
{
    public class NotPermittedFileExtensionException : ExceptionBase
    {
        public NotPermittedFileExtensionException(string fileName) : base($"File {fileName} extension is not permitted")
        {

        }
        public override string Code => Constants.ErrorCodes.NotPermittedFileExtension;

        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}
