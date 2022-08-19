using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.Wallet.Exceptions
{
    public class VestingWalletNotFoundException : ExceptionBase
    {
        public VestingWalletNotFoundException(string id) : base($"Vesting wallet with id {id} not found")
        {
        }

        public override string Code => Constants.ErrorCodes.VestingWalletNotFound;
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}