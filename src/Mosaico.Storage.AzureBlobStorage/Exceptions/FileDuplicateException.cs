using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Storage.AzureBlobStorage.Exceptions
{
    public class FileDuplicateException : ExceptionBase
    {
        public FileDuplicateException(string id) : base($"File with id {id} already exists")
        {
        }

        public override string Code => Constants.ErrorCodes.DuplicateFile;
        public override int StatusCode => StatusCodes.Status409Conflict;
    }
}