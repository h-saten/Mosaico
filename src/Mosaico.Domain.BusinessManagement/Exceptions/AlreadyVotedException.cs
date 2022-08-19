using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.BusinessManagement.Exceptions
{
    public class AlreadyVotedException : ExceptionBase
    {
        public AlreadyVotedException(string address) : base($"Address {address} already voted")
        {
        }

        public override string Code => Constants.ErrorCodes.AlreadyVoted;
        public override int StatusCode => StatusCodes.Status409Conflict;
    }
}