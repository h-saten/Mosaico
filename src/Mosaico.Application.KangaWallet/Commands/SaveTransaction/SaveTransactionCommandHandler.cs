using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.KangaWallet.Services;
using Mosaico.BackgroundJobs.Base;
using Mosaico.Domain.Wallet.Abstractions;
using Serilog;

namespace Mosaico.Application.KangaWallet.Commands.SaveTransaction
{
    public class SaveTransactionCommandHandler : IRequestHandler<SaveTransactionCommand>
    {
        private readonly ILogger _logger;
        private readonly IWalletDbContext _walletDbContext;
        private readonly IBackgroundJobProvider _backgroundJob;
        private readonly IKangaTransactionRepository _transactionRepository;
        
        public SaveTransactionCommandHandler(
            IWalletDbContext walletDbContext, 
            IBackgroundJobProvider backgroundJob, 
            IKangaTransactionRepository transactionRepository, 
            ILogger logger = null) 
        {
            _walletDbContext = walletDbContext;
            _backgroundJob = backgroundJob;
            _transactionRepository = transactionRepository;
            _logger = logger;
        }
    
        public async Task<Unit> Handle(SaveTransactionCommand request, CancellationToken cancellationToken)
        {
            _logger?.Information($"Start saving transaction: '{request.TransactionId}' from Kanga.");
            
            var transactionExist =
                await _walletDbContext
                    .Transactions
                    .AnyAsync(m => m.CorrelationId == request.TransactionId,
                    cancellationToken);
            
            if (transactionExist)
            {
                _logger?.Error($"Transaction with id: '{request.TransactionId}' already exist");
                return await Task.FromResult(new Unit());
            }

            _backgroundJob.Execute(() => _transactionRepository.SaveAsync(request.TransactionId));
            return await Task.FromResult(new Unit());
        }
    }
}
