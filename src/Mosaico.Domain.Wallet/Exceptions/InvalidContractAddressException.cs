using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.Wallet.Exceptions
{
    public class InvalidContractAddress : ExceptionBase
    {
        public string ContractAddress;
        public InvalidContractAddress(string address) : base($"Invalid contract address: '{address}'.")
        {
            ContractAddress = address;
        }
        
        public override string Code => Constants.ErrorCodes.InvalidContractAddress;
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}