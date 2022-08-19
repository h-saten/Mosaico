using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaico.Application.ProjectManagement.Exceptions
{
    public class ProjectCompanyNotFoundException : ExceptionBase
    {
        public Guid ProjectId { get; set; }
        
        public ProjectCompanyNotFoundException(Guid projectId): base($"Company for project: '{projectId}' not found.")
        {
            ProjectId = projectId;
        }
        public override string Code => Constants.ErrorCodes.CompanyNotExists;

        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}
