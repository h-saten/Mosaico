using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Core.ResourceManager.Exceptions
{
    public class ResourceCategoryNotFoundException : ExceptionBase
    {
        public ResourceCategoryNotFoundException(string category) : base($"Resource map {category} not found")
        {
        }

        public override string Code => Constants.ErrorCodes.ResourceCategoryNotFound;
        public override int StatusCode => StatusCodes.Status500InternalServerError;
    }
}