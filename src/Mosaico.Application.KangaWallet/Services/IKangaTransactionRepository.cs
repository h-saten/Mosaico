using System.Threading.Tasks;

namespace Mosaico.Application.KangaWallet.Services
{
    public interface IKangaTransactionRepository
    {
        public Task SaveAsync(string transactionId);
    }
}