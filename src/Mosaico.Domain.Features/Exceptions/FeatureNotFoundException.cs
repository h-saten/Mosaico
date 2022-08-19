using System;
using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.Features.Exceptions
{
    public class FeatureNotFoundException : ExceptionBase
    {
        public FeatureNotFoundException(string name) : base($"Feature {name} not found")
        {
        }

        public FeatureNotFoundException(Guid id) : base($"Feature with ID {id} not found")
        {
        }

        public override string Code => Constants.ErrorCodes.FeatureNotFound;
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}