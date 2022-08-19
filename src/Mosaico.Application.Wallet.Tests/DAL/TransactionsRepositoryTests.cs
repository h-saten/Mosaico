using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using Hangfire;
using Moq;
using Mosaico.Application.Wallet.DAL;
using Mosaico.Application.Wallet.Tests.Factories.Domain;
using Mosaico.Blockchain.Base.DAL;
using Mosaico.Domain.Mongodb.Base.Repository;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Repositories;
using Mosaico.Persistence.SqlServer.Contexts.Wallet;
using Mosaico.Tests.Base;
using NUnit.Framework;

namespace Mosaico.Application.Wallet.Tests.DAL
{
    public class TransactionsRepositoryTests : EFInMemoryTestBase
    {
        protected override List<Profile> Profiles { get; }

        protected override void RegisterDependencies(ContainerBuilder builder)
        {
            base.RegisterDependencies(builder);
            RegisterContext<WalletContext>(builder);
            builder.RegisterModule(new WalletApplicationModule());
            GlobalConfiguration.Configuration.UseInMemoryStorage();
            CurrentUserContext = CreateCurrentUserContextMock();
            builder.RegisterInstance(CurrentUserContext).AsImplementedInterfaces();
        }
        
        [Test]
        [TestCase(0, 0, 1)]
        [TestCase(1, 0, 1)]
        [TestCase(1, 1, 1)]
        [TestCase(5, 0, 1)]
        [TestCase(6, 5, 0)]
        public async Task ShouldReturnValidTransactionsAmountForLastAttemptDate(int attemptCounter, int currentDateSecondsSubtract, int expectedTransactionAmount)
        {
            //Arrange
            var context = GetContext<IWalletDbContext>();
            var userId = CurrentUserContext.UserId;
            var wallet = context.CreateWallet(userId);
            var token = context.CreateToken();
            var transactionStatusPending = context.CreatePending();
            var transactionPurchaseType = context.CreatePurchase();
            var transaction1 = context.CreateTransaction(userId, token.Id, transactionPurchaseType, transactionStatusPending);
            transaction1.ConfirmationAttemptsCounter = attemptCounter;
            transaction1.LastConfirmationAttemptedAt = DateTimeOffset.Now;
            transaction1.NextConfirmationAttemptAt = DateTimeOffset.UtcNow.AddSeconds(-currentDateSecondsSubtract);
            transaction1.WalletAddress = wallet.AccountAddress;
            await context.SaveChangesAsync();
            
            var sut = new TransactionsRepository(context, new Mock<ITransactionRepository>().Object);
            
            // Act
            var transactions = await sut.TransactionsWaitingForConfirmationAsync();
            
            // Assert
            Assert.AreEqual(expectedTransactionAmount, transactions.Count);
        }
        
    }
}