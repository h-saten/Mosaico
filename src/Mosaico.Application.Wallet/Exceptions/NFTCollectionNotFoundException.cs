using System;
using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.Wallet.Exceptions
{
    public class NFTCollectionNotFoundException : ExceptionBase
    {
        public NFTCollectionNotFoundException(Guid id) : base($"NFT Collection {id} not found")
        {
        }

        public override string Code => "NFT_COLLECTION_NOT_FOUND";
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}