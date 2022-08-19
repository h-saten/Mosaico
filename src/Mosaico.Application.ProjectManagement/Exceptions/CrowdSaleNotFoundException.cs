using System;
using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.ProjectManagement.Exceptions
{
    public class CrowdSaleNotFoundException : ExceptionBase
    {
        public CrowdSaleNotFoundException(string id) : base($"Project: '{id}' crowdsale not found")
        {
        }
        
        public CrowdSaleNotFoundException(Guid id) : base($"Project: '{id}' crowdsale not found")
        {
        }

        public override string Code => Constants.ErrorCodes.ProjectCrowdSaleNotFound;
        public override int StatusCode => StatusCodes.Status404NotFound;    
    }
}
