using System.Threading.Tasks;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Application.Wallet.Services.Abstractions
{
    public interface ITransactionService
    {
        Task ConfirmTransactionAsync(Transaction transaction, string hash);
        Task FailTransactionAsync(Transaction transaction, string error);
    }
}