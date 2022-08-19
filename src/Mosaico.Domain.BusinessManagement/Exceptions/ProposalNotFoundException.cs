using System;
using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.BusinessManagement.Exceptions
{
    public class ProposalNotFoundException : ExceptionBase
    {
        public ProposalNotFoundException(Guid message) : base($"Proposal {message} not found")
        {
        }

        public override string Code => Constants.ErrorCodes.ProposalNotFound;
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}