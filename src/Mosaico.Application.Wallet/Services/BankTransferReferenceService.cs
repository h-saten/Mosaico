using System;
using System.Linq;
using System.Threading.Tasks;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Application.Wallet.Services
{
    public class BankTransferReferenceService : IBankTransferReferenceService
    {
        private readonly IWalletDbContext _walletDbContext;

        public BankTransferReferenceService(IWalletDbContext walletDbContext)
        {
            _walletDbContext = walletDbContext;
        }

        public async Task<string> GenerateReferenceAsync(ProjectBankPaymentDetails bankDetails, string currency, decimal tokenAmount, decimal fiatValue, string userId)
        {
            var uniqueSeed = Guid.NewGuid().ToString().Split('-').LastOrDefault();
            var projectSeed = bankDetails.ProjectId.ToString().Split('-').FirstOrDefault();
            var referenceCode = $"MS{projectSeed}-{uniqueSeed}";
            var reference = new ProjectBankTransferTitle
            {
                Reference = referenceCode,
                UserId = userId,
                ProjectBankPaymentDetails = bankDetails,
                ProjectBankPaymentDetailsId = bankDetails.Id,
                Currency = currency,
                FiatAmount = fiatValue,
                TokenAmount = tokenAmount
            };
            bankDetails.ProjectBankTransferTitles.Add(reference);
            await _walletDbContext.SaveChangesAsync();
            return referenceCode;
        }
    }
}