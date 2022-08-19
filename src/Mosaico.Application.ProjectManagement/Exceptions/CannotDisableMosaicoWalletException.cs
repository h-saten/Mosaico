using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.ProjectManagement.Exceptions
{
    public class CannotDisableMosaicoWalletException : ExceptionBase
    {
        public CannotDisableMosaicoWalletException() : base($"Cannot disable Mosaico Wallet")
        {
        }

        public override string Code => "CANNOT_DISABLE_MOSAICO";
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}