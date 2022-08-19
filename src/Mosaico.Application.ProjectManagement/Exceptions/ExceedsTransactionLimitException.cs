using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.ProjectManagement.Exceptions
{
    public class ExceedsTransactionLimitException : ExceptionBase
    {
        public ExceedsTransactionLimitException(string userId) : base($"User {userId} exceeds daily transaction limit")
        {
        }

        public override string Code => "DAILY_TRANSACTION_LIMIT_EXCEEDED";
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}