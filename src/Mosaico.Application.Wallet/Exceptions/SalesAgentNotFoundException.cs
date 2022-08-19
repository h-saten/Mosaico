using System;
using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.Wallet.Exceptions
{
    public class SalesAgentNotFoundException : ExceptionBase
    {
        public SalesAgentNotFoundException(Guid id) : base($"Sales agent {id} was not found")
        {
        }

        public override string Code => "SALES_AGENT_NOT_FOUND";
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}