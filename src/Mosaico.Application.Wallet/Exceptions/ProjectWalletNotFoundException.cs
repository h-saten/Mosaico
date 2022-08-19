using System;
using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.Wallet.Exceptions
{
    public class ProjectWalletNotFoundException : ExceptionBase
    {
        public ProjectWalletNotFoundException(string network, Guid projectId) : base($"Project wallet {projectId} not found in network {network}")
        {
        }

        public override string Code => "PROJECT_WALLET_NOT_FOUND";
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}