using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.API.Base.Exceptions
{
    public class UnsupportedModuleException : ExceptionBase
    {
        public UnsupportedModuleException(string moduleName) : base(
            $"Configuration for {moduleName} was not identified. Check your appsettings.")
        {
        }

        public override string Code => Constants.ErrorCodes.UnsupportedModule;
        public override int StatusCode => StatusCodes.Status500InternalServerError;
    }
}