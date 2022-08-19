using System;
using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.Wallet.Exceptions
{
    public class VaultNotFoundException : ExceptionBase
    {
        public VaultNotFoundException(Guid id) : base($"Vault {id} not found")
        {
        }

        public override string Code => "VAULT_NOT_FOUND";
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}