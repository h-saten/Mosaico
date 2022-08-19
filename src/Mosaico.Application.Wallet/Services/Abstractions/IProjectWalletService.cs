using System;
using System.Threading.Tasks;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Application.Wallet.Services.Abstractions
{
    public interface IProjectWalletService
    {
        Task<ProjectWallet> CreateWalletAsync(string network, Guid projectId);
        Task<ProjectWalletAccount> GetAccountAsync(string network, Guid projectId, string userId);
    }
}