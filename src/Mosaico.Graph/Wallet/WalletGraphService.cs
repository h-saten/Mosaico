using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Graph.Abstractions;
using Mosaico.Graph.Wallet.Repositories;

namespace Mosaico.Graph.Wallet
{
    public class WalletGraphService : IWalletGraphService
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly IMapper _mapper;
        private readonly ITransactionReadonlyRepository _transactionReadonlyRepository;
        
        public WalletGraphService(IWalletDbContext walletDbContext, ITransactionReadonlyRepository transactionReadonlyRepository, IMapper mapper)
        {
            _walletDbContext = walletDbContext;
            _transactionReadonlyRepository = transactionReadonlyRepository;
            _mapper = mapper;
        }

        public async Task SyncTransactionAsync(Guid transactionId)
        {
            throw new NotImplementedException();
            // var transaction = await _walletDbContext.Transactions.FirstOrDefaultAsync(t => t.Id == transactionId);
            // if (transaction == null)
            // {
            //     _transactionReadonlyRepository.Remove(transactionId);
            //     await _transactionReadonlyRepository.SaveChangesAsync();
            // }
            // else
            // {
            //     var transactionModel = _mapper.Map<Transaction>(transaction);
            // }
        }

        public Task SyncTransactionsAsync(List<Guid> transactionIds)
        {
            throw new NotImplementedException();
        }
    }
}