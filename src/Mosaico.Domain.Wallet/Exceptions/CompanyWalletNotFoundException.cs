using System;
using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.Wallet.Exceptions
{
    public class CompanyWalletNotFoundException : ExceptionBase
    {
        public CompanyWalletNotFoundException(string companyId) : base($"Wallet for company of ID {companyId} not found")
        {
        }
        
        public CompanyWalletNotFoundException(string address, string chain) : base($"Wallet with address '{address}' on '{chain}' chain not found.")
        {
        }
        
        public CompanyWalletNotFoundException(Guid companyId, string chain) : base($"Wallet of company {companyId} on '{chain}' chain not found.")
        {
        }

        //TODO: to constants
        public override string Code => "COMPANY_WALLET_NOT_FOUND";
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}