using System.Threading.Tasks;

namespace Mosaico.Blockchain.Base.DAL
{
    public interface IBlockWithTransactionRepository
    {
        Task AddAsync<T>(string blockchain, string blockHash, T item);
        Task<T> GetAsync<T>(string blockchain, string blockHash);
    }
}