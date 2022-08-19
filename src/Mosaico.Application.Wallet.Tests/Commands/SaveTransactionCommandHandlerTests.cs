using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using FizzWare.NBuilder;
using KangaExchange.SDK.Abstractions;
using KangaExchange.SDK.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using Mosaico.Application.KangaWallet.Commands.SaveTransaction;
using Mosaico.Application.KangaWallet.Services;
using Mosaico.BackgroundJobs.Base;
using Mosaico.Base.Tools;
using Mosaico.Domain.Mongodb.Base.Abstractions;
using Mosaico.Domain.Mongodb.Base.Repository;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Events.Base;
using Mosaico.Persistence.SqlServer.Contexts.Wallet;
using Mosaico.SDK.Identity.Abstractions;
using Mosaico.SDK.Identity.Models;
using Mosaico.Tests.Base;
using NUnit.Framework;
using Serilog;

namespace Mosaico.Application.Wallet.Tests.Commands
{
    public class SaveTransactionCommandHandlerTests : EFInMemoryTestBase
    {
        protected override List<Profile> Profiles => new()
        {
            new WalletMapperProfile()
        };

        private Mock<ILogger> _logger;
        private Mock<IBackgroundJobProvider> _backgroundJobProvider;
        private Mock<IKangaTransactionRepository> _transactionRepository;

        protected override void RegisterDependencies(ContainerBuilder builder)
        {
            base.RegisterDependencies(builder);
            RegisterContext<WalletContext>(builder);
            builder.RegisterModule(new WalletApplicationModule());
            
            _logger = new Mock<ILogger>();
            builder.RegisterInstance(_logger.Object).AsImplementedInterfaces();
            CurrentUserContext = CreateCurrentUserContextMock();

            _backgroundJobProvider = new Mock<IBackgroundJobProvider>();
            _backgroundJobProvider.Setup(x => x.Execute(It.IsAny<Expression<Func<Task>>>()))
                .Returns("");
            
            _transactionRepository = new Mock<IKangaTransactionRepository>();
            _transactionRepository.Setup(x => x.SaveAsync(It.IsAny<string>()))
                .Returns(Task.CompletedTask);
        }

        private async Task<string> SaveKangaTransactionAsync(IWalletDbContext walletDbContext)
        {
            var userId = CurrentUserContext.UserId;
            var token = Builder<Token>.CreateNew().Build();
            token.Id = Guid.NewGuid();
            walletDbContext.Tokens.Add(token);
            var wallet = Builder<CompanyWallet>.CreateNew().Build();
            wallet.Id = Guid.NewGuid();
            wallet.CompanyId = Guid.Parse("0b214de7-8958-4956-8eed-28f9ba2c47c6");
            wallet.Network = Blockchain.Base.Constants.BlockchainNetworks.Ethereum;
            walletDbContext.CompanyWallets.Add(wallet);

            var transactionCorrelationId = Guid.NewGuid().ToString();
            var transaction1 = new Transaction
            {
                Currency = "PLN",
                CorrelationId = transactionCorrelationId,
                FailureReason = null,
                InitiatedAt = DateTimeOffset.UtcNow.AddDays(-1),
                FinishedAt = DateTimeOffset.UtcNow,
                TokenId = token.Id,
                TokenAmount = 30,
                PaymentProcessor = "KANGA_EXCHANGE",
                UserId = userId,
                Status = new TransactionStatus(Domain.Wallet.Constants.TransactionStatuses.Confirmed, "Confirmed"),
                Type = new TransactionType(Domain.Wallet.Constants.TransactionType.Purchase, "Purchase"),
                WalletAddress = wallet.AccountAddress,
                Network = wallet.Network
            };
            walletDbContext.Transactions.Add(transaction1);
            await walletDbContext.SaveChangesAsync();

            return transaction1.CorrelationId;
        }

        private async Task<string> SaveKangaTransactionOnMongoDbAsync(IWalletDbContext walletDbContext)
        {
            var userId = CurrentUserContext.UserId;
            var token = Builder<Token>.CreateNew().Build();
            token.Id = Guid.NewGuid();
            walletDbContext.Tokens.Add(token);
            var wallet = Builder<CompanyWallet>.CreateNew().Build();
            wallet.Id = Guid.NewGuid();
            wallet.CompanyId = Guid.Parse("0b214de7-8958-4956-8eed-28f9ba2c47c6");
            wallet.Network = Blockchain.Base.Constants.BlockchainNetworks.Ethereum;
            walletDbContext.CompanyWallets.Add(wallet);

            var transactionCorrelationId = Guid.NewGuid().ToString();
            var transaction1 = new Transaction
            {
                Currency = "PLN",
                CorrelationId = transactionCorrelationId,
                FailureReason = null,
                InitiatedAt = DateTimeOffset.UtcNow.AddDays(-1),
                FinishedAt = DateTimeOffset.UtcNow,
                TokenId = token.Id,
                TokenAmount = 30,
                PaymentProcessor = "KANGA_EXCHANGE",
                UserId = userId,
                Status = new TransactionStatus(Domain.Wallet.Constants.TransactionStatuses.Confirmed, "Confirmed"),
                Type = new TransactionType(Domain.Wallet.Constants.TransactionType.Purchase, "Purchase"),
                WalletAddress = wallet.AccountAddress,
                Network = wallet.Network
            };

            return transaction1.CorrelationId;
        }

        [Test]
        public async Task ShouldStopWhenTransactionWasAlreadySaved()
        {
            //Arrange
            var walletDbContext = GetContext<WalletContext>();

            var transactionCorrelationId = await SaveKangaTransactionAsync(walletDbContext);

            var transactionMongoDb = await SaveKangaTransactionOnMongoDbAsync(walletDbContext);
            var command = new SaveTransactionCommand
            {
                TransactionId = transactionCorrelationId
            };
            
            var handler = new SaveTransactionCommandHandler(
                walletDbContext,
                _backgroundJobProvider.Object,
                _transactionRepository.Object,
                _logger.Object);

            // act
            await handler.Handle(command, CancellationToken.None);
            
            // assert
            _backgroundJobProvider.Verify(x => x.Execute(It.IsAny<Expression<Func<Task>>>()), Times.Never);
            await Task.CompletedTask;
        }
        
        [Test]
        public async Task ShouldExecuteJobOnlyOnce()
        {
            //Arrange
            var walletDbContext = GetContext<WalletContext>();
            var command = new SaveTransactionCommand
            {
                TransactionId = Guid.NewGuid().ToString()
            };
            
            var handler = new SaveTransactionCommandHandler(
                walletDbContext,
                _backgroundJobProvider.Object,
                _transactionRepository.Object,
                _logger.Object);

            // act
            await handler.Handle(command, CancellationToken.None);
            
            // assert
            _backgroundJobProvider.Verify(x => x.Execute(It.IsAny<Expression<Func<Task>>>()), Times.Once);
            await Task.CompletedTask;
        }
    }
}