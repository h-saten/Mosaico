using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mosaico.Graph.Abstractions
{
    public interface IWalletGraphService
    {
        Task SyncTransactionAsync(Guid transactionId);
        Task SyncTransactionsAsync(List<Guid> transactionIds);
    }
}