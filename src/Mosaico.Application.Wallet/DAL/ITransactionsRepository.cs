using System.Collections.Generic;
using System.Threading.Tasks;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Application.Wallet.DAL
{
    public interface ITransactionsRepository
    {
        Task<IList<Transaction>> TransactionsWaitingForConfirmationAsync();
        Task<ulong> TransactionBlockNumberByHashAsync(string chain, string transactionHash);
    }
}