using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.Wallet.Exceptions.Funds
{
    public class FundNotFoundException : ExceptionBase
    {
        public FundNotFoundException(string message) : base($"Venture fund {message} not found")
        {
        }

        public override string Code => "VENTURE_FUND_NOT_FOUND";
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}