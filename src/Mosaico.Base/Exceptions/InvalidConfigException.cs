using Microsoft.AspNetCore.Http;

namespace Mosaico.Base.Exceptions
{
    public class InvalidConfigException : ExceptionBase
    {
        public InvalidConfigException(string configName) : base($"Config {configName} is invalid. Please check app settings")
        {
        }

        public override string Code => Constants.ExceptionCodes.InvalidConfiguration;
        public override int StatusCode => StatusCodes.Status500InternalServerError;
    }
}