using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.Features.Exceptions
{
    public class FeatureMustHaveAName : ExceptionBase
    {
        public FeatureMustHaveAName() : base($"Feature must have a name")
        {
        }

        public override string Code => Constants.ErrorCodes.FeatureMustHaveAName;
        public override int StatusCode => StatusCodes.Status409Conflict;
    }
}