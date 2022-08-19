using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaico.API.Base.Exceptions
{
    public class FileSizeLimitExceededException : ExceptionBase
    {
        public FileSizeLimitExceededException(long fileSize, long limit): base($"File size limit exceeded {limit} bytes by {fileSize - limit} bytes")
        {
        }

        public override string Code => Constants.ErrorCodes.FileSizeLimitExceeded;
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}
