using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaico.Application.ProjectManagement.Exceptions
{
    public class CompanyNotExistsException : ExceptionBase
    {
        public CompanyNotExistsException(string comapnyId): base($"There is no company with ${comapnyId} id")
        {

        }
        public override string Code => Constants.ErrorCodes.CompanyNotExists;

        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}
