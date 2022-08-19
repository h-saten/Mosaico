using System;
using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.Wallet.Exceptions
{
    public class VaultTransferInProgressException : ExceptionBase
    {
        public VaultTransferInProgressException(Guid vaultId) : base($"Vault {vaultId} has another operation in progress")
        {
        }

        public override string Code => "VAULT_OPERATION_IN_PROGRESS";
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}