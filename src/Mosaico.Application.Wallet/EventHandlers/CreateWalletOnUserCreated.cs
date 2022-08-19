using System;
using System.Threading.Tasks;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Events.Base;
using Mosaico.Events.Identity;
using Mosaico.Events.Wallet;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.SDK.Identity.Abstractions;
using Serilog;

namespace Mosaico.Application.Wallet.EventHandlers
{
    [EventInfo(nameof(CreateWalletOnUserCreated),  "users:api")]
    [EventTypeFilter(typeof(UserCreatedEvent))]
    public class CreateWalletOnUserCreated : EventHandlerBase
    {
        private readonly IUserManagementClient _managementClient;
        private readonly ILogger _logger;
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventFactory _eventFactory;
        private readonly IWalletDbContext _walletDbContext;
        private readonly IEthereumClientFactory _ethereumClient;

        public CreateWalletOnUserCreated(
            IUserManagementClient managementClient, 
            IEventPublisher eventPublisher, 
            IEventFactory eventFactory, 
            IWalletDbContext walletDbContext, IEthereumClientFactory ethereumClient, ILogger logger = null)
        {
            _managementClient = managementClient;
            _eventPublisher = eventPublisher;
            _eventFactory = eventFactory;
            _walletDbContext = walletDbContext;
            _ethereumClient = ethereumClient;
            _logger = logger;
        }

        public override async Task HandleAsync(CloudEvent @event)
        {
            var userEvent = @event.GetData<UserCreatedEvent>();
            if (userEvent != null)
            {
                var user = await _managementClient.GetUserAsync(userEvent.Id);
                if (user != null)
                {
                    var configs = _ethereumClient.GetAllConfigurations();
                    foreach (var config in configs)
                    {
                        var walletId = await CreateWalletAsync(user.Id, config.Chain);
                        var createdEvent = _eventFactory.CreateEvent(Events.Wallet.Constants.EventPaths.Wallets,
                            new WalletCreatedEvent
                            {
                                WalletId = walletId
                            });
                        await _eventPublisher.PublishAsync(createdEvent.Source, createdEvent);
                    }
                }
                else
                    _logger?.Warning($"User ${userEvent.Id} was not found in database");
            }
            else
            {
                _logger?.Warning($"event {@event.Type} received empty payload");
            }
        }

        private async Task<Guid> CreateWalletAsync(string userId, string network)
        {
            var client = _ethereumClient.GetClient(network);
            var account = client.CreateWallet();

            var walletEntity = new Domain.Wallet.Entities.Wallet
            {
                UserId = userId,
                Network = network,
                PrivateKey = account.PrivateKey,
                AccountAddress = account.Address,
                PublicKey = account.PublicKey
            };

            _walletDbContext.Wallets.Add(walletEntity);
            await _walletDbContext.SaveChangesAsync();

            return walletEntity.Id;
        }
    }
}