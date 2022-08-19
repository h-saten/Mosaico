using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.BusinessManagement.Exceptions
{
    public class CompanyAlreadyExistsException : ExceptionBase
    {
        public CompanyAlreadyExistsException(string name) : base($"Company {name} already exists")
        {
        }

        public override string Code => Constants.ErrorCodes.CompanyAlreadyExists;
        public override int StatusCode => StatusCodes.Status409Conflict;
    }
}