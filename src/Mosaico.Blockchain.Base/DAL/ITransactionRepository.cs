using System.Threading.Tasks;
using Mosaico.Blockchain.Base.DAL.Models;

namespace Mosaico.Blockchain.Base.DAL
{
    public interface ITransactionRepository
    {
        Task<TransactionReceipt> TransactionByHashAsync(string chain, string transactionHash);
    }
}