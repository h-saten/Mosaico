using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.ProjectManagement.Exceptions
{
    public class VestingWalletNotDeployedException : ExceptionBase
    {
        public VestingWalletNotDeployedException(string fundId) : base($"Vesting wallet was not deployed for fund {fundId}")
        {
        }

        public override string Code => Constants.ErrorCodes.VestingWalletWasNotDeployed;
        public override int StatusCode => StatusCodes.Status500InternalServerError;
    }
}