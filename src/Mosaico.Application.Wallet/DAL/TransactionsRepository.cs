using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Blockchain.Base.DAL;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Application.Wallet.DAL
{
    public class TransactionsRepository : ITransactionsRepository
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly ITransactionRepository _blockchainTransactionRepository;

        public TransactionsRepository(IWalletDbContext walletDbContext, ITransactionRepository blockchainTransactionRepository)
        {
            _walletDbContext = walletDbContext;
            _blockchainTransactionRepository = blockchainTransactionRepository;
        }

        public async Task<IList<Transaction>> TransactionsWaitingForConfirmationAsync()
        {
            // TODO maybe should be moved to configuration 
            var maxConfirmationAttempts = 6;
            var currentDate = DateTimeOffset.UtcNow;
            
            var query = _walletDbContext
                .Transactions
                .Where(t => 
                    t.Type.Key == Domain.Wallet.Constants.TransactionType.Purchase 
                    && t.Status.Key == Domain.Wallet.Constants.TransactionStatuses.Pending
                    && t.ConfirmationAttemptsCounter < maxConfirmationAttempts
                    && (
                        t.LastConfirmationAttemptedAt == null 
                        || (t.NextConfirmationAttemptAt != null && t.NextConfirmationAttemptAt <= currentDate)
                        )
                )
                .OrderBy(t => t.CreatedAt)
                .Take(100);
                
            return await query.ToListAsync();
        }

        public async Task<ulong> TransactionBlockNumberByHashAsync(string chain, string transactionHash)
        {
            var transactionDetails =
                await _blockchainTransactionRepository.TransactionByHashAsync(chain, transactionHash);

            return transactionDetails.BlockNumber;
        }
    }
}