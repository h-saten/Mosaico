using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.DocumentManagement.Exceptions
{
    public class LogoNotFoundException : ExceptionBase
    {
        public LogoNotFoundException(string id) : base($"Logo for {id} not found")
        {

        }
        public override string Code => Constants.ErrorCodes.DocumentNotFound;

        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}