using System;
using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.ProjectManagement.Exceptions
{
    public class FundNotFoundException : ExceptionBase
    {
        public FundNotFoundException(string id) : base($"Fund {id} not found")
        {
        }
        
        public FundNotFoundException(Guid id) : base($"Fund {id} not found")
        {
        }

        public override string Code => Constants.ErrorCodes.FundNotFound;
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}