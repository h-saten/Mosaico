using System;
using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.Wallet.Exceptions
{
    public class WalletNotFoundException : ExceptionBase
    {
        public WalletNotFoundException(string walletId) : base($"Wallet for user of ID {walletId} not found")
        {
        }
        public WalletNotFoundException(string address, string chain) : base($"Wallet with address '{address}' on '{chain}' chain not found.")
        {
        }
        
        public WalletNotFoundException(Guid userId, string chain) : base($"Wallet of user {userId} on '{chain}' chain not found.")
        {
        }

        //TODO: to constants
        public override string Code => "WALLET_NOT_FOUND";
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}