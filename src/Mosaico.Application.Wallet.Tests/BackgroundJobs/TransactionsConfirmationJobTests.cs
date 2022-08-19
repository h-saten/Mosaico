using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using FizzWare.NBuilder;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Moq;
using Mosaico.Application.Wallet.BackgroundJobs;
using Mosaico.Application.Wallet.DAL;
using Mosaico.Application.Wallet.Tests.Factories.Domain;
using Mosaico.Authorization.Base;
using Mosaico.Blockchain.Base.DAL;
using Mosaico.Blockchain.Base.DAL.Models;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Persistence.SqlServer.Contexts.Wallet;
using Mosaico.Tests.Base;
using NUnit.Framework;

namespace Mosaico.Application.Wallet.Tests.BackgroundJobs
{
    public class TransactionsConfirmationJobTests : EFInMemoryTestBase
    {
        protected override List<Profile> Profiles { get; }
        private ITransactionsRepository _transactionsRepository;
        private ICurrentUserContext _currentUserContext;

        protected override void RegisterDependencies(ContainerBuilder builder)
        {
            base.RegisterDependencies(builder);
            var context = RegisterContext<WalletContext>(builder);
            builder.RegisterModule(new WalletApplicationModule());

            var transactionRepository = new Mock<ITransactionRepository>();

            _transactionsRepository = new TransactionsRepository(context, transactionRepository.Object);
            _currentUserContext = CreateCurrentUserContextMock();
           
            GlobalConfiguration.Configuration.UseInMemoryStorage();
        }

        [Test]
        public async Task ShouldSkipLoopWhenIsLackOfTransactions()
        {
            //Arrange
            var context = GetContext<IWalletDbContext>();
            var moralisApiClient = new Mock<ITokenRepository>();

            var sut = new TransactionsConfirmationJob(context, moralisApiClient.Object, _transactionsRepository);

            //Act
            await sut.ExecuteAsync();
            
            // Assert
            moralisApiClient
                .Verify(mock => 
                    mock.Erc20TransfersAsync(
                        It.IsAny<string>(), 
                        It.IsAny<string>(), 
                        It.IsAny<ulong>(), 
                        It.IsAny<ulong>()), 
                    Times.Never);
        }
        
        [Test]
        public async Task ShouldIncreaseAttemptsCounter()
        {
            //Arrange
            var context = GetContext<IWalletDbContext>();
            var userId = _currentUserContext.UserId;
            var token = context.CreateToken();
            var transactionStatusPending = context.CreatePending();
            var transactionPurchaseType = context.CreatePurchase();
            var transaction = context.CreateTransaction(userId, token.Id, transactionPurchaseType, transactionStatusPending);
            await context.SaveChangesAsync();
            
            var moralisApiClient = new Mock<ITokenRepository>();
            moralisApiClient.Setup(x => x.Erc20TransfersAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<ulong>(),
                    It.IsAny<ulong>()))
                .ReturnsAsync(new List<ERC20Transfer>
                {
                    Builder<ERC20Transfer>.CreateNew().Build()
                });
            
            var sut = new TransactionsConfirmationJob(context, moralisApiClient.Object, _transactionsRepository);

            //Act
            await sut.ExecuteAsync();
            
            // Assert
            moralisApiClient
                .Verify(mock => 
                        mock.Erc20TransfersAsync(
                            It.IsAny<string>(), 
                            It.IsAny<string>(), 
                            It.IsAny<ulong>(),
                            It.IsAny<ulong>()), 
                    Times.Once);

            var transactionResult = await context
                .Transactions
                .AsNoTracking()
                .Include(m => m.Status)
                .Where(t => t.Id == transaction.Id)
                .SingleOrDefaultAsync();
            
            Assert.NotNull(transactionResult);
            Assert.NotNull(transactionResult.Status.Title == Domain.Wallet.Constants.TransactionStatuses.Pending);
            Assert.AreEqual(1, transactionResult.ConfirmationAttemptsCounter);
        }

        [Test]
        public async Task ShouldIncreaseAttemptsCounterToValue2()
        {
            //Arrange
            var context = GetContext<IWalletDbContext>();
            var userId = _currentUserContext.UserId;
            var token = context.CreateToken();
            var transactionStatusPending = context.CreatePending();
            var transactionPurchaseType = context.CreatePurchase();
            var transaction = context.CreateTransaction(userId, token.Id, transactionPurchaseType, transactionStatusPending);
            await context.SaveChangesAsync();
            
            var moralisApiClient = new Mock<ITokenRepository>();
            moralisApiClient.Setup(x => x.Erc20TransfersAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<ulong>(),
                    It.IsAny<ulong>()))
                .ReturnsAsync(new List<ERC20Transfer>
                {
                    Builder<ERC20Transfer>.CreateNew().Build()
                });
            
            var sut = new TransactionsConfirmationJob(context, moralisApiClient.Object, _transactionsRepository);

            //Act
            await sut.ExecuteAsync();
            
            var transactionAfterFirstExecution = await context
                .Transactions
                .Where(t => t.Id == transaction.Id)
                .SingleOrDefaultAsync();
            
            transactionAfterFirstExecution.NextConfirmationAttemptAt = DateTimeOffset.UtcNow.AddMinutes(-1);
            await context.SaveChangesAsync();
            
            await sut.ExecuteAsync();

            // Assert
            var transactionResult = await context
                .Transactions
                .Include(m => m.Status)
                .AsNoTracking()
                .Where(t => t.Id == transaction.Id)
                .SingleOrDefaultAsync();
            
            Assert.NotNull(transactionResult);
            Assert.NotNull(transactionResult.Status.Title == Domain.Wallet.Constants.TransactionStatuses.Pending);
            Assert.AreEqual(2, transactionResult.ConfirmationAttemptsCounter);
        }

        [Test]
        public async Task ShouldConfirmTransaction()
        {
            //Arrange
            var context = GetContext<IWalletDbContext>();
            var userId = _currentUserContext.UserId;
            var wallet = context.CreateWallet(userId);
            var token = context.CreateToken();
            var transactionStatusPending = context.CreatePending();
            var transactionPurchaseType = context.CreatePurchase();
            var transaction1 = context.CreateTransaction(userId, token.Id, transactionPurchaseType, transactionStatusPending);
            transaction1.WalletAddress = wallet.AccountAddress;
            var transaction2 = context.CreateTransaction(userId, token.Id, transactionPurchaseType, transactionStatusPending);
            await context.SaveChangesAsync();

            var erc20Transfer = Builder<ERC20Transfer>.CreateNew().Build();
            erc20Transfer.Value = (decimal) transaction1.TokenAmount;
            erc20Transfer.ToAddress = transaction1.WalletAddress;
            
            var moralisApiClient = new Mock<ITokenRepository>();
            moralisApiClient.Setup(x => x.Erc20TransfersAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<ulong>(),
                    It.IsAny<ulong>()))
                .ReturnsAsync(new List<ERC20Transfer>
                {
                    erc20Transfer
                });
            
            var sut = new TransactionsConfirmationJob(context, moralisApiClient.Object, _transactionsRepository);

            //Act
            await sut.ExecuteAsync();
            
            // Assert
            moralisApiClient
                .Verify(mock => 
                        mock.Erc20TransfersAsync(
                            It.IsAny<string>(), 
                            It.IsAny<string>(), 
                            It.IsAny<ulong>(),
                            It.IsAny<ulong>()), 
                    Times.Once);

            var transaction1Result = await context
                .Transactions
                .Include(m => m.Status)
                .AsNoTracking()
                .Where(t => t.Id == transaction1.Id)
                .SingleOrDefaultAsync();

            var transaction2Result = await context
                .Transactions
                .Include(m => m.Status)
                .AsNoTracking()
                .Where(t => t.Id == transaction2.Id)
                .SingleOrDefaultAsync();
            
            Assert.NotNull(transaction1Result);
            Assert.NotNull(transaction2Result);
            Assert.NotNull(transaction1Result.Status.Title == Domain.Wallet.Constants.TransactionStatuses.Confirmed);
            Assert.NotNull(transaction2Result.Status.Title == Domain.Wallet.Constants.TransactionStatuses.Pending);
            Assert.AreEqual(1, transaction1Result.ConfirmationAttemptsCounter);
            Assert.AreEqual(1, transaction2Result.ConfirmationAttemptsCounter);
        }
    }
}