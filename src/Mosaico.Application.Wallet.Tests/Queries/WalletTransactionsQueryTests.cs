using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using FizzWare.NBuilder;
using Moq;
using Mosaico.Application.Wallet.Queries.WalletTransactions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Events.Base;
using Mosaico.Persistence.SqlServer.Contexts.Wallet;
using Mosaico.SDK.Identity.Abstractions;
using Mosaico.Tests.Base;
using Mosaico.Validation.Base;
using NUnit.Framework;

namespace Mosaico.Application.Wallet.Tests.Queries
{
    public class WalletTransactionsQueryTests : EFInMemoryTestBase
    {
        protected override List<Profile> Profiles { get; }
        private Mock<IEventPublisher> _eventPublisherMock;
        private Mock<IUserManagementClient> _managementClientMock;
        
        protected override void RegisterDependencies(ContainerBuilder builder)
        {
            base.RegisterDependencies(builder);
            RegisterContext<WalletContext>(builder);
            builder.RegisterModule(new WalletApplicationModule());
            
            _eventPublisherMock = new Mock<IEventPublisher>();
            _eventPublisherMock.Setup(p => p.PublishAsync(It.IsAny<string>(), It.IsAny<CloudEvent>())).Verifiable();
            builder.RegisterInstance(_eventPublisherMock.Object).AsImplementedInterfaces();

            _managementClientMock = new Mock<IUserManagementClient>();
            builder.RegisterInstance(_managementClientMock.Object).As<IUserManagementClient>();
            
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new WalletMapperProfile());
            });
            
            var mapper = mockMapper.CreateMapper();
            builder.RegisterInstance(mapper).AsImplementedInterfaces();
            CurrentUserContext = CreateCurrentUserContextMock();
            builder.RegisterInstance(CurrentUserContext).AsImplementedInterfaces();
        }

        [Test]
        public async Task ShouldReturnMappedToDtoTransaction()
        {
            //arrange
            var walletDbContext = GetContext<IWalletDbContext>();
            var userId = CurrentUserContext.UserId;

            var token = Builder<Token>.CreateNew().Build();
            token.Id = Guid.NewGuid();
            walletDbContext.Tokens.Add(token);
            
            var wallet = Builder<Domain.Wallet.Entities.Wallet>.CreateNew().Build();
            wallet.Id = Guid.NewGuid();
            wallet.UserId = userId;
            wallet.Network = Blockchain.Base.Constants.BlockchainNetworks.Ethereum;
            walletDbContext.Wallets.Add(wallet);

            var transaction1 = new Transaction
            {
                Currency = "PLN",
                CorrelationId = Guid.NewGuid().ToString(),
                FailureReason = null,
                InitiatedAt = DateTimeOffset.UtcNow.AddDays(-1),
                FinishedAt = DateTimeOffset.UtcNow,
                TokenId = token.Id,
                TokenAmount = 20,
                PaymentProcessor = "KangaExchange",
                UserId = userId,
                Status = new TransactionStatus(Domain.Wallet.Constants.TransactionStatuses.Confirmed, "Confirmed"),
                Type = new TransactionType(Domain.Wallet.Constants.TransactionType.Purchase, "Purchase"),
                WalletAddress = wallet.AccountAddress,
                Network = wallet.Network
            };

            walletDbContext.Transactions.Add(transaction1);
            
            await walletDbContext.SaveChangesAsync();
            
            //act
            var query = new WalletTransactionsQuery
            {
                Network =  wallet.Network,
                WalletAddress = wallet.AccountAddress
            };
            
            var result = await SendAsync(query);
            
            var resultTransaction = result.Entities.ElementAt(0);
            Assert.NotNull(resultTransaction);
            Assert.AreEqual(1, result.Total);
            Assert.AreEqual(token.Symbol, resultTransaction.Token.Symbol);
        }
    }
}