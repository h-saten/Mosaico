using System.Threading.Tasks;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Application.Wallet.Services.Abstractions
{
    public interface IBankTransferReferenceService
    {
        Task<string> GenerateReferenceAsync(ProjectBankPaymentDetails bankDetails, string currency, decimal tokenAmount,
            decimal fiatValue, string userId);
    }
}