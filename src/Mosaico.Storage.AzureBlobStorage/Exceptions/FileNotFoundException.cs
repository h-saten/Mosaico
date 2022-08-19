using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Storage.AzureBlobStorage.Exceptions
{
    public class FileNotFoundException : ExceptionBase
    {
        public FileNotFoundException(string id) : base($"File with ID {id} not found")
        {
        }

        public override string Code => Constants.ErrorCodes.FileNotFound;
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}