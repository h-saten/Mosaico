using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Base.Tools;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Application.Wallet.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IWalletDbContext _walletDbContext;

        public TransactionService(IDateTimeProvider dateTimeProvider, IWalletDbContext walletDbContext)
        {
            _dateTimeProvider = dateTimeProvider;
            _walletDbContext = walletDbContext;
        }
        
        public async Task FailTransactionAsync(Transaction transaction, string error) 
        {
            transaction.FinishedAt = _dateTimeProvider.Now();
            transaction.FailureReason = error;
            var status = await _walletDbContext.TransactionStatuses.FirstOrDefaultAsync(s =>
                s.Key == Domain.Wallet.Constants.TransactionStatuses.Failed);
            transaction.SetStatus(status);
            _walletDbContext.Transactions.Update(transaction);
            await _walletDbContext.SaveChangesAsync();
        }

        public async Task ConfirmTransactionAsync(Transaction transaction, string hash)
        {
            transaction.TransactionHash = hash;
            transaction.FinishedAt = _dateTimeProvider.Now();
            var status = await _walletDbContext.TransactionStatuses.FirstOrDefaultAsync(s =>
                s.Key == Domain.Wallet.Constants.TransactionStatuses.Confirmed);
            transaction.SetStatus(status);
            _walletDbContext.Transactions.Update(transaction);
            await _walletDbContext.SaveChangesAsync();
        }
    }
}