using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Core.ResourceManager.Exceptions
{
    public class ResourceNotFoundException : ExceptionBase
    {
        public ResourceNotFoundException(string name) : base($"Resource {name} was not found")
        {
        }

        public override string Code => Constants.ErrorCodes.ResourceNotFound;
        public override int StatusCode => StatusCodes.Status500InternalServerError;
    }
}