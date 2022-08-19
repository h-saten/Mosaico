using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mosaico.Blockchain.Base.DAL.Models;

namespace Mosaico.Blockchain.Base.DAL
{
    public interface ICrowdSaleRepository
    {
        Task<List<PurchaseConfirmation>> PurchaseConfirmationsAsync(string contractAddress, string chain, DateTimeOffset? fromDate = null);
    }
}