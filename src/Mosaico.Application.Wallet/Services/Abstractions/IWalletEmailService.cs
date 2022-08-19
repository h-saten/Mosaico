using System.Threading.Tasks;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.SDK.ProjectManagement.Models;

namespace Mosaico.Application.Wallet.Services.Abstractions
{
    public interface IWalletEmailService
    {
        Task SendTransactionFinishedAsync(Transaction transaction, MosaicoProject project, Token token, string recipient, string language = Base.Constants.Languages.English);

        Task SendBankTransferDetailsAsync(Transaction transaction, MosaicoProject project, Token token,
            ProjectBankTransferTitle transferTitle, string recipient, string language = Base.Constants.Languages.English);

        Task SendStakingRewardPaid(Token token, string recipient, decimal amount, PaymentCurrency currency,
            string language = Base.Constants.Languages.English);

        Task SendStakingActivatedAsync(string tokenSymbol, string recipient, decimal amount, decimal apr,
            string language = Base.Constants.Languages.English);

        Task SendStakingDeactivatedAsync(string tokenSymbol, string recipient, decimal amount,
            string language = Base.Constants.Languages.English);

        Task SendStakingRewardPaid(PaymentCurrency token, string recipient, decimal amount, PaymentCurrency currency,
            string language = Base.Constants.Languages.English);
    }
}