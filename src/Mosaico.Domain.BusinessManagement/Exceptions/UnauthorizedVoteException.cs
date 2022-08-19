using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.BusinessManagement.Exceptions
{
    public class UnauthorizedVoteException : ExceptionBase
    {
        public UnauthorizedVoteException(string message) : base(message)
        {
        }

        public override string Code => Constants.ErrorCodes.UnauthorizedVote;
        public override int StatusCode => StatusCodes.Status403Forbidden;
    }
}