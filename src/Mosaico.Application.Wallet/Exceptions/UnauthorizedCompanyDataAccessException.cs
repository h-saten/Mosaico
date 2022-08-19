using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.Wallet.Exceptions
{
    public class UnauthorizedCompanyDataAccess : ExceptionBase
    {
        public UnauthorizedCompanyDataAccess(string message) : base(message)
        {
        }

        public override string Code => Constants.ErrorCodes.UnauthorizedCompanyDataAccess;
        public override int StatusCode => StatusCodes.Status401Unauthorized;
    }
}