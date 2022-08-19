using System;
using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.BusinessManagement.Exceptions
{
    public class CompanyRoleNotFoundException : ExceptionBase
    {
        public CompanyRoleNotFoundException(string name) : base($"Company role {name} not found")
        {
        }

        public CompanyRoleNotFoundException(Guid id) : base($"Company role with ID {id} not found")
        {
        }

        public override string Code => Constants.ErrorCodes.CompanyRoleNotFound;
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}