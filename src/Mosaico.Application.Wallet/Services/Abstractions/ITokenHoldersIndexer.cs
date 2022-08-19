using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mosaico.Application.Wallet.Services.Abstractions
{
    public interface ITokenHoldersIndexer
    {
        Task<Dictionary<string, decimal>> UpdateHoldersAsync(Guid tokenId, CancellationToken cancellationToken = new());
    }
}