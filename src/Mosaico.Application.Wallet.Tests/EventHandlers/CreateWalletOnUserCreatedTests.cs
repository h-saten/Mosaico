using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using Mosaico.Application.Wallet.EventHandlers;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Events.Base;
using Mosaico.Events.Identity;
using Mosaico.Events.Wallet;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.Persistence.SqlServer.Contexts.Wallet;
using Mosaico.SDK.Identity.Abstractions;
using Mosaico.SDK.Identity.Models;
using Mosaico.Tests.Base;
using Nethereum.Signer;
using Nethereum.Web3.Accounts;
using NUnit.Framework;
using Serilog;

namespace Mosaico.Application.Wallet.Tests.EventHandlers
{
    public class CreateWalletOnUserCreatedTests : EFInMemoryTestBase
    {
        protected override List<Profile> Profiles { get; }

        protected override void RegisterDependencies(ContainerBuilder builder)
        {
            base.RegisterDependencies(builder);
            RegisterContext<WalletContext>(builder);
            builder.RegisterModule(new WalletApplicationModule());
        }

        [Test]
        public async Task ShouldCreateWalletForUserAndSaveInDatabase()
        {
            //Arrange
            var userId = Guid.NewGuid().ToString();
            var ethereumClient = new Mock<IEthereumClient>();
            var accountMock = new Mock<Account>();
            ethereumClient.Setup(m => m.CreateWallet()).Returns(new Account(Guid.NewGuid().ToString(), Chain.Private));
            var managementClient = new Mock<IUserManagementClient>();
            managementClient.Setup(m => m.GetUserAsync(It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(new MosaicoUser
            {
                Id = userId
            });
            var factoryMock = new Mock<IEthereumClientFactory>();
            factoryMock.Setup(f => f.GetClient(It.IsAny<string>())).Returns(ethereumClient.Object);
            var eventPublisher = new Mock<IEventPublisher>();
            var eventFactory = new Mock<IEventFactory>();
            eventFactory.Setup(m => m.CreateEvent(It.IsAny<string>(), It.IsAny<object>())).Returns(new CloudEvent());
            var logger = new Mock<ILogger>();

            var ef = new CloudEventFactory();
            var eventPayload = new UserCreatedEvent(userId);
            var @event = ef.CreateEvent(Events.Identity.Constants.EventPaths.Users, eventPayload);
            var walletDbContext = GetContext<IWalletDbContext>();
            var handler = new CreateWalletOnUserCreated(managementClient.Object, eventPublisher.Object, eventFactory.Object, walletDbContext, factoryMock.Object, logger.Object);

            //Act
            await handler.HandleAsync(@event);

            var walletEntities = await walletDbContext
                .Wallets
                .Where(m => m.UserId == userId)
                .SingleOrDefaultAsync();
            
            //Assert
            eventFactory.Verify(v => v.CreateEvent(It.Is<string>(m => m == Events.Wallet.Constants.EventPaths.Wallets), It.IsAny<WalletCreatedEvent>()));
            Assert.NotNull(walletEntities);
        }
    }
}