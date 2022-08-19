using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.ProjectManagement.Exceptions
{
    public class InvestmentPackageNotFoundException : ExceptionBase
    {
        public InvestmentPackageNotFoundException(string id) : base($"Investment package {id} not found")
        {
        }

        public override string Code => Constants.ErrorCodes.InvestmentPackageNotFound;
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}