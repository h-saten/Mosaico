using System;
using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.Wallet.Exceptions
{
    public class VaultAlreadyExistsException : ExceptionBase
    {
        public VaultAlreadyExistsException(Guid tokenId) : base($"Vault of token {tokenId} already exists")
        {
        }

        public override string Code => "VAULT_ALREADY_EXISTS";
        public override int StatusCode => StatusCodes.Status409Conflict;
    }
}