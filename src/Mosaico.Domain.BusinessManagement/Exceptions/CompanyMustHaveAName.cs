using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.BusinessManagement.Exceptions
{
    public class CompanyMustHaveAName : ExceptionBase
    {
        public CompanyMustHaveAName() : base($"Company must have a name")
        {
        }

        public override string Code => Constants.ErrorCodes.CompanyMustHaveAName;
        public override int StatusCode => StatusCodes.Status409Conflict;
    }
}