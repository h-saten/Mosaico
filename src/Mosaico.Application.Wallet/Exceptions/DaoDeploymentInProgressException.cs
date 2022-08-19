using System;
using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.Wallet.Exceptions
{
    public class DaoDeploymentInProgressException : ExceptionBase
    {
        public DaoDeploymentInProgressException(Guid companyId) : base($"Company {companyId} already has DAO in deployment")
        {
        }

        public override string Code => "DAO_DEPLOYMENT_IN_PROGRESS";
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}