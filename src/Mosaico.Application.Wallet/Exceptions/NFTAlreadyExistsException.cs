using System;
using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.Wallet.Exceptions
{
    public class NFTAlreadyExistsException : ExceptionBase
    {
        public NFTAlreadyExistsException(string tokenId) : base($"NFT with token id {tokenId} already exists")
        {
        }

        public override string Code => "NFT_ALREADY_EXISTS";
        public override int StatusCode => StatusCodes.Status409Conflict;
    }
}