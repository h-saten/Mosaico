using System;
using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.Wallet.Exceptions
{
    public class VaultDeployingException : ExceptionBase
    {
        public VaultDeployingException(Guid tokenId) : base($"Vault for token {tokenId} is in deploying")
        {
        }

        public override string Code => "VAULT_DEPLOYING";
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}