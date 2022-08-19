using System;
using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.ProjectManagement.Exceptions.Affiliation
{
    public class AffiliationIsNotActiveException : ExceptionBase
    {
        public AffiliationIsNotActiveException(Guid projectId) : base($"Project {projectId} has disabled affiliation")
        {
        }

        public override string Code => "AFFILIATION_DISABLED";
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}