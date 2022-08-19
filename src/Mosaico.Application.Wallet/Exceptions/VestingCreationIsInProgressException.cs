using System;
using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.Wallet.Exceptions
{
    public class VestingCreationIsInProgressException : ExceptionBase
    {
        public VestingCreationIsInProgressException(Guid vaultId) : base($"Vesting operation for vault {vaultId} is in progress")
        {
        }

        public override string Code => "VESTING_OPERATION_IN_PROGRESS";
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}