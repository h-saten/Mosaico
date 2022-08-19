using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.Wallet.Exceptions
{
    public class InsufficientFundsException : ExceptionBase
    {
        public InsufficientFundsException(string account) : base($"Insufficient funds in account {account}")
        {
        }

        public override string Code => Constants.ErrorCodes.InsufficientFunds;
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}