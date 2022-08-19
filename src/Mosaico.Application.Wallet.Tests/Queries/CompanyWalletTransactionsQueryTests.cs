using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using FizzWare.NBuilder;
using Moq;
using Mosaico.Application.Wallet.Queries.Company.CompanyWalletTransactions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Persistence.SqlServer.Contexts.Wallet;
using Mosaico.SDK.Identity.Abstractions;
using Mosaico.SDK.Identity.Models;
using Mosaico.Tests.Base;
using NUnit.Framework;

namespace Mosaico.Application.Wallet.Tests.Queries
{
    public class CompanyWalletTransactionsQueryTests : EFInMemoryTestBase
    {
        protected override List<Profile> Profiles { get; }
        private Mock<IUserManagementClient> _managementClientMock;
        
        protected override void RegisterDependencies(ContainerBuilder builder)
        {
            base.RegisterDependencies(builder);
            RegisterContext<WalletContext>(builder);
            builder.RegisterModule(new WalletApplicationModule());
            
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new WalletMapperProfile());
            });
            var mapper = mockMapper.CreateMapper();
            builder.RegisterInstance(mapper).AsImplementedInterfaces();
            _managementClientMock = new Mock<IUserManagementClient>();
            _managementClientMock
                .Setup(c => c.GetUserPermissionsAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(new List<MosaicoPermission>()
                {
                    new MosaicoPermission()
                    {
                        Id = Guid.NewGuid(),
                        Key = Authorization.Base.Constants.Permissions.Company.CanReadCompanyWallet,
                        EntityId = Guid.Parse("0b214de7-8958-4956-8eed-28f9ba2c47c6")
                    }
                });
            builder.RegisterInstance(_managementClientMock.Object).As<IUserManagementClient>();
            CurrentUserContext = CreateCurrentUserContextMock();
            builder.RegisterInstance(CurrentUserContext).AsImplementedInterfaces();
        }

        [Test]
        public async Task ShouldReturnMappedToDtoTransactionForACompany()
        {
            //arrange
            var walletDbContext = GetContext<IWalletDbContext>();
            var userId = CurrentUserContext.UserId;

            var token = Builder<Token>.CreateNew().Build();
            token.Id = Guid.NewGuid();
            walletDbContext.Tokens.Add(token);
            
            var wallet = Builder<CompanyWallet>.CreateNew().Build();
            wallet.Id = Guid.NewGuid();
            wallet.CompanyId = Guid.Parse("0b214de7-8958-4956-8eed-28f9ba2c47c6");
            wallet.Network = Blockchain.Base.Constants.BlockchainNetworks.Ethereum;
            walletDbContext.CompanyWallets.Add(wallet);

            var transaction1 = new Transaction
            {
                Currency = "PLN",
                CorrelationId = Guid.NewGuid().ToString(),
                FailureReason = null,
                InitiatedAt = DateTimeOffset.UtcNow.AddDays(-1),
                FinishedAt = DateTimeOffset.UtcNow,
                TokenId = token.Id,
                TokenAmount = 30,
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
            var query = new CompanyWalletTransactionsQuery
            {
                CompanyId = wallet.CompanyId
            };
            var result = await SendAsync(query);
            
            //assert
            var resultTransaction = result.Entities.ElementAt(0);
            Assert.NotNull(resultTransaction);
            Assert.AreEqual(1, result.Total);
            Assert.AreEqual(token.Symbol, resultTransaction.Token.Symbol);
        }
    }
}