using System;
using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.Wallet.Exceptions
{
    public class DepositIsDeployingException : ExceptionBase
    {
        public DepositIsDeployingException(Guid distributionId) : base($"Deposit for {distributionId} is deploying")
        {
        }

        public override string Code => "DEPOSIT_DEPLOYING";
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}