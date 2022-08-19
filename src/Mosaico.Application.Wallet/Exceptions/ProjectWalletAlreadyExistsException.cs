using System;
using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.Wallet.Exceptions
{
    public class ProjectWalletAlreadyExistsException : ExceptionBase
    {
        public ProjectWalletAlreadyExistsException(string network, Guid projectId) : base($"Wallet for project {projectId} already exists in network {network}")
        {
        }

        public override string Code => "PROJECT_WALLET_ALREADY_EXISTS";
        public override int StatusCode => StatusCodes.Status409Conflict;
    }
}