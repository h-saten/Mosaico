using System;
using System.Collections.Generic;
using System.Linq;
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
using Mosaico.Base.Tools;
using Mosaico.Blockchain.Base.DAL;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Events.Base;
using Mosaico.Persistence.SqlServer.Contexts.Wallet;
using Mosaico.SDK.Identity.Abstractions;
using Mosaico.SDK.Identity.Models;
using Mosaico.SDK.ProjectManagement.Abstractions;
using Mosaico.Tests.Base;
using NUnit.Framework;
using Serilog;

namespace Mosaico.Application.Wallet.Tests.Services
{
    public class KangaTransactionRepositoryTests : EFInMemoryTestBase
    {
        protected override List<Profile> Profiles => new()
        {
            new WalletMapperProfile()
        };

        private Mock<ILogger> _logger;
        private DateTimeProvider _dateTimeProvider;
        private Mock<IEventPublisher> _eventPublisher;
        private Mock<IEventFactory> _eventFactory;
        private Mock<IUserManagementClient> _userManagementClient;
        private Mock<IProjectManagementClient> _projectManagementClient;
        private Mock<IExchangeRateRepository> _exchangeRateRepository;

        protected override void RegisterDependencies(ContainerBuilder builder)
        {
            base.RegisterDependencies(builder);
            RegisterContext<WalletContext>(builder);
            builder.RegisterModule(new WalletApplicationModule());
            
            _logger = new Mock<ILogger>();
            builder.RegisterInstance(_logger.Object).AsImplementedInterfaces();
            CurrentUserContext = CreateCurrentUserContextMock();

            _dateTimeProvider = new DateTimeProvider();
            
            _eventPublisher = new Mock<IEventPublisher>();
            
            _eventFactory = new Mock<IEventFactory>();
            _eventFactory.Setup(m => m.CreateEvent(It.IsAny<string>(), It.IsAny<object>())).Returns(new CloudEvent());
            
            _userManagementClient = new Mock<IUserManagementClient>();
            var userId = CurrentUserContext.UserId;
            var userEmail = CurrentUserContext.Email;
            _userManagementClient.Setup(x => x.GetUserByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MosaicoUser
                {
                    Id = userId,
                    Email = userEmail,
                    FirstName = "Chris",
                    IsAMLVerified = true,
                    LastName = "Test"
                });
                
            _projectManagementClient = new Mock<IProjectManagementClient>();
            
            _exchangeRateRepository = new Mock<IExchangeRateRepository>();
            _exchangeRateRepository.Setup(x => x.GetExchangeRateAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new decimal(1));
        }

        private async Task<(Transaction, Token)> SavePendingTransactionAsync(IWalletDbContext walletDbContext)
        {
            var token = Builder<Token>.CreateNew().Build();
            token.Id = Guid.NewGuid();
            walletDbContext.Tokens.Add(token);
            var wallet = Builder<CompanyWallet>.CreateNew().Build();
            wallet.Id = Guid.NewGuid();
            wallet.CompanyId = Guid.Parse("0b214de7-8958-4956-8eed-28f9ba2c47c6");
            wallet.Network = Blockchain.Base.Constants.BlockchainNetworks.Ethereum;
            walletDbContext.CompanyWallets.Add(wallet);
            var userId = CurrentUserContext.UserId;

            var transaction1 = new Transaction
            {
                Currency = "USDT",
                CorrelationId = null,
                FailureReason = null,
                InitiatedAt = DateTimeOffset.UtcNow.AddDays(-1),
                FinishedAt = null,
                TokenId = token.Id,
                TokenAmount = 30,
                PayedAmount = 60,
                PaymentProcessor = "KANGA_EXCHANGE",
                UserId = userId,
                Status = new TransactionStatus(Domain.Wallet.Constants.TransactionStatuses.Confirmed, "Confirmed"),
                Type = new TransactionType(Domain.Wallet.Constants.TransactionType.Purchase, "Purchase"),
                WalletAddress = wallet.AccountAddress,
                Network = wallet.Network,
                CreatedAt = DateTimeOffset.UtcNow.AddDays(-1)
            };
            walletDbContext.Transactions.Add(transaction1);
            await walletDbContext.SaveChangesAsync();
            return (transaction1, token);
        }
        
        [Test]
        public async Task ShouldUpdatePendingTransaction()
        {
            //Arrange
            var userEmail = CurrentUserContext.Email;
            var walletDbContext = GetContext<WalletContext>();
            var (transaction, token) = await SavePendingTransactionAsync(walletDbContext);
            
            var kangaBuyApiClient = new Mock<IKangaBuyApiClient>();
            kangaBuyApiClient.Setup(x => x.GetTransaction(It.IsAny<string>()))
                .ReturnsAsync(new TransactionResponseDto
                {
                    Amount = (decimal) transaction.PayedAmount,
                    Code = 0,
                    Currency = transaction.Currency,
                    Email = userEmail,
                    Quantity = (decimal) transaction.TokenAmount,
                    Result = "ok",
                    Status = "CONFIRMED",
                    Token = token.Symbol,
                    BuyCode = string.Empty
                });
            
            var sut = new KangaTransactionRepository(
                kangaBuyApiClient.Object, 
                walletDbContext, 
                _userManagementClient.Object,
                _eventFactory.Object,
                _eventPublisher.Object,
                _dateTimeProvider,
                _projectManagementClient.Object,
                _exchangeRateRepository.Object,
                _logger.Object);

            var transactionsAmountBeforeTest = await walletDbContext
                .Transactions
                .CountAsync();
                
            var transactionCorrelationId = Guid.NewGuid().ToString();

            // act
            await sut.SaveAsync(transactionCorrelationId);
            
            var transactionsAmountAfterTest = await walletDbContext
                .Transactions
                .CountAsync();
            
            var processedTransaction = await walletDbContext
                .Transactions
                .Where(x => x.CorrelationId == transactionCorrelationId)
                .SingleOrDefaultAsync();
            
            Assert.NotNull(processedTransaction);
            Assert.AreEqual(transactionsAmountBeforeTest, transactionsAmountAfterTest);
        }
        
        [Test]
        public async Task ShouldCreateNewTransaction()
        {
            //Arrange
            var userId = CurrentUserContext.UserId;
            var userEmail = CurrentUserContext.Email;
            var walletDbContext = GetContext<WalletContext>();

            var token = Builder<Token>.CreateNew().Build();
            token.Id = Guid.NewGuid();
            walletDbContext.Tokens.Add(token);
            
            await walletDbContext.SaveChangesAsync();
            
            var kangaBuyApiClient = new Mock<IKangaBuyApiClient>();
            kangaBuyApiClient.Setup(x => x.GetTransaction(It.IsAny<string>()))
                .ReturnsAsync(new TransactionResponseDto
                {
                    Amount = 50,
                    Code = 0,
                    Currency = "USDT",
                    Email = userEmail,
                    Quantity = 100,
                    Result = "ok",
                    Status = "CONFIRMED",
                    Token = token.Symbol,
                    BuyCode = string.Empty
                });
            
            var sut = new KangaTransactionRepository(
                kangaBuyApiClient.Object, 
                walletDbContext, 
                _userManagementClient.Object,
                _eventFactory.Object,
                _eventPublisher.Object,
                _dateTimeProvider,
                _projectManagementClient.Object,
                _exchangeRateRepository.Object,
                _logger.Object);
        
            var transactionCorrelationId = Guid.NewGuid().ToString();

            // act
            await sut.SaveAsync(transactionCorrelationId);
            
            var processedTransaction = await walletDbContext
                .Transactions
                .Where(x => x.CorrelationId == transactionCorrelationId)
                .SingleOrDefaultAsync();
            
            Assert.NotNull(processedTransaction);
            Assert.AreEqual(processedTransaction.UserId, userId);
            Assert.AreEqual(processedTransaction.TokenAmount, 100);
            Assert.AreEqual(processedTransaction.TokenId, token.Id);
            Assert.AreEqual(processedTransaction.CorrelationId, transactionCorrelationId);
        }
    }
}