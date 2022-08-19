using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.ProjectManagement.Exceptions
{
    public class UnsupportedPaymentCurrencyException : ExceptionBase
    {
        public string ContractAddress { get; set; }
        
        public UnsupportedPaymentCurrencyException(string contractAddress) : base($"Currency with address: '{contractAddress}' is not supported")
        {
            ContractAddress = contractAddress;
        }

        public override string Code => Constants.ErrorCodes.UnsupportedPaymentCurrency;
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}