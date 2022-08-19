using System;
using System.Threading.Tasks;
using Mosaico.Base.Tools;
using Mosaico.Domain.Statistics.Abstractions;
using Mosaico.Domain.Statistics.Entities;
using Mosaico.Events.Base;
using Mosaico.Events.Wallet;
using Mosaico.SDK.Wallet.Abstractions;
using Serilog;

namespace Mosaico.Application.Statistics.EventHandlers
{
    [EventInfo(nameof(SaveProjectTransactionOnTransactionFinalized), "wallets:api")]
    [EventTypeFilter(typeof(PurchaseTransactionConfirmedEvent))]
    public class SaveProjectTransactionOnTransactionFinalized : EventHandlerBase
    {
        private readonly ILogger _logger;
        private readonly IStatisticsDbContext _context;
        private readonly IWalletClient _walletClient;
        private readonly IDateTimeProvider _dateTimeProvider;

        public SaveProjectTransactionOnTransactionFinalized(
            IStatisticsDbContext context, 
            IWalletClient walletClient, 
            IDateTimeProvider dateTimeProvider, 
            ILogger logger = null)
        {
            _context = context;
            _walletClient = walletClient;
            _dateTimeProvider = dateTimeProvider;
            _logger = logger;
        }

        public override async Task HandleAsync(CloudEvent @event)
        {
            var eventData = @event?.GetData<PurchaseTransactionConfirmedEvent>();
            if (eventData is null)
            {
                return;
            }
            
            var usdtAmount = eventData.Payed;
            if (eventData.Currency != "USDT")
            {
                var exchangeRate = await _walletClient.ExchangeRateAsync(eventData.Currency);
                usdtAmount *= exchangeRate;
            }

            var entity = new PurchaseTransaction
            {
                Currency = eventData.Currency,
                Id = Guid.NewGuid(),
                Payed = eventData.Payed,
                TokenId = eventData.TokenId,
                TokensAmount = eventData.TokensAmount,
                TransactionId = eventData.TransactionId,
                UserId = string.IsNullOrWhiteSpace(eventData.UserId) ? Guid.Empty : Guid.Parse(eventData.UserId),
                USDTAmount = usdtAmount,
                Date = _dateTimeProvider.Now()
            };

            await _context.PurchaseTransactions.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
    }
}