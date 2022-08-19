using System;
using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.BusinessManagement.Exceptions
{
    public class CompanyNotFoundException : ExceptionBase
    {
        public CompanyNotFoundException(string name) : base($"Company {name} not found")
        {
        }

        public CompanyNotFoundException(Guid id) : base($"Company with ID {id} not found")
        {
        }

        public override string Code => Constants.ErrorCodes.CompanyNotFound;
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}