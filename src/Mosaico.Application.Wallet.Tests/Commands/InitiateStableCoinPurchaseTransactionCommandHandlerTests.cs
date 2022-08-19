using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Features.Indexed;
using AutoMapper;
using FizzWare.NBuilder;
using Microsoft.EntityFrameworkCore;
using Moq;
using Mosaico.Application.ProjectManagement.Permissions;
using Mosaico.Application.Wallet.Commands.Transactions.InitiateStableCoinPurchaseTransaction;
using Mosaico.Application.Wallet.Exceptions;
using Mosaico.Application.Wallet.Services;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Application.Wallet.Tests.Factories;
using Mosaico.Application.Wallet.Tests.Factories.Domain;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.Domain.Wallet.ValueObjects;
using Mosaico.Events.Base;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.Persistence.SqlServer.Contexts.Wallet;
using Mosaico.SDK.ProjectManagement.Abstractions;
using Mosaico.SDK.ProjectManagement.Models;
using Mosaico.Tests.Base;
using NUnit.Framework;
using Serilog;

namespace Mosaico.Application.Wallet.Tests.Commands
{
    public class InitiateStableCoinPurchaseTransactionCommandHandlerTests : EFInMemoryTestBase
    {
        protected override List<Profile> Profiles => new()
        {
            new WalletMapperProfile()
        };

        private Mock<ILogger> _logger;
        private Mock<IProjectManagementClient> _projectManagementClient;
        private Mock<IStakingService> _stakingService;

        protected override void RegisterDependencies(ContainerBuilder builder)
        {
            base.RegisterDependencies(builder);
            RegisterContext<WalletContext>(builder);
            builder.RegisterModule(new WalletApplicationModule());
            
            _logger = new Mock<ILogger>();
            builder.RegisterInstance(_logger.Object).AsImplementedInterfaces();
            _projectManagementClient = new Mock<IProjectManagementClient>();
            builder.RegisterInstance(_projectManagementClient.Object).AsImplementedInterfaces();
            _stakingService = new Mock<IStakingService>();
            _stakingService.Setup(s => s.StakeAsync(It.IsAny<string>(),It.IsAny<Action<ContractStakingConfiguration>>()));
            builder.RegisterInstance(_stakingService.Object).AsImplementedInterfaces();
        }
        
        private Mock<IIndex<string, ITokenService>> ResolveTokenServices()
        {
            var tokenService = new Mock<ITokenService>();
            tokenService.Setup(s => s.SetWalletAllowanceAsync(Blockchain.Base.Constants.BlockchainNetworks.Default, It.IsAny<Action<AllowanceConfiguration>>()))
                .Returns(Task.CompletedTask);

            var tokenServices = new Mock<IIndex<string, ITokenService>>();
            var tokenServiceObject = tokenService.Object;
            tokenServices.Setup(x => x[It.IsAny<string>()]).Returns(tokenService.Object);
            tokenServices.Setup(x => x.TryGetValue(It.IsAny<string>(), out tokenServiceObject))
                .Returns(true);
                
            return tokenServices;
        }

        private Mock<IProjectManagementClient> ResolveProjectManagementClientReturningValue(Token token = null, bool saleInProgress = false)
        {
            var projectDetails = Builder<MosaicoProjectDetails>.CreateNew().Build();
            projectDetails.SaleInProgress = saleInProgress;
            projectDetails.TokenId = token?.Id;
            var projectManagementClient = new Mock<IProjectManagementClient>();
            projectManagementClient
                .Setup(m => m.GetProjectDetailsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(projectDetails);
            return projectManagementClient;
        }

        [Test]
        public async Task ShouldThrowExceptionWhenProjectNotFound()
        {
            //Arrange
            var walletDbContext = GetContext<WalletContext>();
            var userContext = CreateCurrentUserContextMock();
            var projectPermissionFactory = new Mock<IProjectPermissionFactory>();
            var projectManagementClient = new Mock<IProjectManagementClient>();
            var userWalletService = new Mock<IUserWalletService>();
            var eventPublisher = new Mock<IEventPublisher>();
            var eventFactory = new Mock<IEventFactory>();
            eventFactory.Setup(m => m.CreateEvent(It.IsAny<string>(), It.IsAny<object>())).Returns(new CloudEvent());
            var logger = new Mock<ILogger>();

            var tokenServices = ResolveTokenServices();
            
            var command = new InitiateStableCoinPurchaseTransactionCommand
            {
                PaymentCurrency = "USDT",
                PaymentProcessor = "KANGA",
                ProjectId = Guid.NewGuid(),
                TokenAmount = 100,
                WalletAddress = EthereumAddressFaker.Generate()
            };
            
            var handler = new InitiateStableCoinPurchaseTransactionCommandHandler(
                eventFactory.Object, 
                eventPublisher.Object, 
                userContext,
                walletDbContext,
                projectPermissionFactory.Object,
                projectManagementClient.Object,
                userWalletService.Object,
                logger.Object);
            
            Assert.ThrowsAsync<ProjectNotFoundException>(async () => await handler.Handle(command, CancellationToken.None));
            await Task.CompletedTask;
        }
        
        [Test]
        public async Task ShouldThrowExceptionWhenProjectSaleNotInProgress()
        {
            //Arrange
            var walletDbContext = GetContext<WalletContext>();
            var userContext = CreateCurrentUserContextMock();
            var projectPermissionFactory = new Mock<IProjectPermissionFactory>();

            var projectManagementClient = ResolveProjectManagementClientReturningValue();
            
            var userWalletService = new Mock<IUserWalletService>();
            var eventPublisher = new Mock<IEventPublisher>();
            var eventFactory = new Mock<IEventFactory>();
            eventFactory.Setup(m => m.CreateEvent(It.IsAny<string>(), It.IsAny<object>())).Returns(new CloudEvent());
            var logger = new Mock<ILogger>();
            
            var tokenServices = ResolveTokenServices();
            
            var command = new InitiateStableCoinPurchaseTransactionCommand
            {
                PaymentCurrency = "USDT",
                PaymentProcessor = "KANGA",
                ProjectId = Guid.NewGuid(),
                TokenAmount = 100,
                WalletAddress = EthereumAddressFaker.Generate()
            };
            
            var handler = new InitiateStableCoinPurchaseTransactionCommandHandler(
                eventFactory.Object, 
                eventPublisher.Object, 
                userContext,
                walletDbContext,
                projectPermissionFactory.Object,
                projectManagementClient.Object,
                userWalletService.Object,
                logger.Object);
            
            Assert.ThrowsAsync<InvalidProjectStatusException>(async () => await handler.Handle(command, CancellationToken.None));
            await Task.CompletedTask;
        }
        
        [Test]
        public async Task ShouldThrowExceptionWhenUserIsNotAllowedToBuy()
        {
            //Arrange
            var walletDbContext = GetContext<WalletContext>();
            var userContext = CreateCurrentUserContextMock();
            var projectPermissionFactory = new Mock<IProjectPermissionFactory>();
            projectPermissionFactory
                .Setup(x => x.GetUserAbilityToPurchaseAsync(
                    It.IsAny<Guid>(), 
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var projectManagementClient = ResolveProjectManagementClientReturningValue(null, true);
            
            var userWalletService = new Mock<IUserWalletService>();
            var eventPublisher = new Mock<IEventPublisher>();
            var eventFactory = new Mock<IEventFactory>();
            eventFactory.Setup(m => m.CreateEvent(It.IsAny<string>(), It.IsAny<object>())).Returns(new CloudEvent());
            var logger = new Mock<ILogger>();
            
            var tokenServices = ResolveTokenServices();

            var command = new InitiateStableCoinPurchaseTransactionCommand
            {
                PaymentCurrency = "USDT",
                PaymentProcessor = "KANGA",
                ProjectId = Guid.NewGuid(),
                TokenAmount = 100,
                WalletAddress = EthereumAddressFaker.Generate()
            };
            
            var handler = new InitiateStableCoinPurchaseTransactionCommandHandler(
                eventFactory.Object, 
                eventPublisher.Object, 
                userContext,
                walletDbContext,
                projectPermissionFactory.Object,
                projectManagementClient.Object,
                userWalletService.Object,
                logger.Object);
            
            Assert.ThrowsAsync<UnauthorizedPurchaseException>(async () => await handler.Handle(command, CancellationToken.None));
            await Task.CompletedTask;
        }
        
        [Test]
        public async Task ShouldThrowExceptionWhenTokenNotFound()
        {
            //Arrange
            var walletDbContext = GetContext<WalletContext>();
            var userContext = CreateCurrentUserContextMock();
            var projectPermissionFactory = new Mock<IProjectPermissionFactory>();
            projectPermissionFactory
                .Setup(x => x.GetUserAbilityToPurchaseAsync(
                    It.IsAny<Guid>(), 
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var projectManagementClient = ResolveProjectManagementClientReturningValue(null, true);

            var userWalletService = new Mock<IUserWalletService>();
            var eventPublisher = new Mock<IEventPublisher>();
            var eventFactory = new Mock<IEventFactory>();
            eventFactory.Setup(m => m.CreateEvent(It.IsAny<string>(), It.IsAny<object>())).Returns(new CloudEvent());
            var logger = new Mock<ILogger>();
            
            var tokenServices = ResolveTokenServices();
            
            var command = new InitiateStableCoinPurchaseTransactionCommand
            {
                PaymentCurrency = "USDT",
                PaymentProcessor = "KANGA",
                ProjectId = Guid.NewGuid(),
                TokenAmount = 100,
                WalletAddress = EthereumAddressFaker.Generate()
            };
            
            var handler = new InitiateStableCoinPurchaseTransactionCommandHandler(
                eventFactory.Object, 
                eventPublisher.Object, 
                userContext,
                walletDbContext,
                projectPermissionFactory.Object,
                projectManagementClient.Object,
                userWalletService.Object,
                logger.Object);
            
            Assert.ThrowsAsync<TokenNotFoundException>(async () => await handler.Handle(command, CancellationToken.None));
            await Task.CompletedTask;
        }
        
        [Test]
        public async Task ShouldThrowExceptionWhenProjectStageNotFound()
        {
            //Arrange
            var walletDbContext = GetContext<WalletContext>();
            var userContext = CreateCurrentUserContextMock();
            var projectPermissionFactory = new Mock<IProjectPermissionFactory>();
            projectPermissionFactory
                .Setup(x => x.GetUserAbilityToPurchaseAsync(
                    It.IsAny<Guid>(), 
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var token = walletDbContext.CreateToken();
            await walletDbContext.SaveChangesAsync();
            var projectManagementClient = ResolveProjectManagementClientReturningValue(token, true); 
            projectManagementClient
                .Setup(m => m.CurrentProjectSaleStage(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ProjectStage) null);
            
            var userWalletService = new Mock<IUserWalletService>();
            var eventPublisher = new Mock<IEventPublisher>();
            var eventFactory = new Mock<IEventFactory>();
            eventFactory.Setup(m => m.CreateEvent(It.IsAny<string>(), It.IsAny<object>())).Returns(new CloudEvent());
            var logger = new Mock<ILogger>();
            
            var tokenServices = ResolveTokenServices();
            
            var command = new InitiateStableCoinPurchaseTransactionCommand
            {
                PaymentCurrency = "USDT",
                PaymentProcessor = "KANGA",
                ProjectId = Guid.NewGuid(),
                TokenAmount = 100,
                WalletAddress = EthereumAddressFaker.Generate()
            };
            
            var handler = new InitiateStableCoinPurchaseTransactionCommandHandler(
                eventFactory.Object, 
                eventPublisher.Object, 
                userContext,
                walletDbContext,
                projectPermissionFactory.Object,
                projectManagementClient.Object,
                userWalletService.Object,
                logger.Object);
            
            Assert.ThrowsAsync<ProjectStageNotExistException>(async () => await handler.Handle(command, CancellationToken.None));
            await Task.CompletedTask;
        }

        [Test]
        public async Task ShouldThrowExceptionWhenPaymentCurrencyNotFound()
        {
            //Arrange
            var walletDbContext = GetContext<WalletContext>();
            var userContext = CreateCurrentUserContextMock();
            var projectPermissionFactory = new Mock<IProjectPermissionFactory>();
            projectPermissionFactory
                .Setup(x => x.GetUserAbilityToPurchaseAsync(
                    It.IsAny<Guid>(), 
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var token = walletDbContext.CreateToken();
            await walletDbContext.SaveChangesAsync();
            var projectManagementClient = ResolveProjectManagementClientReturningValue(token, true); 
            var projectStage = Builder<ProjectStage>.CreateNew().Build();
            projectManagementClient
                .Setup(m => m.CurrentProjectSaleStage(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(projectStage);
            
            var userWalletService = new Mock<IUserWalletService>();
            var eventPublisher = new Mock<IEventPublisher>();
            var eventFactory = new Mock<IEventFactory>();
            eventFactory.Setup(m => m.CreateEvent(It.IsAny<string>(), It.IsAny<object>())).Returns(new CloudEvent());
            var logger = new Mock<ILogger>();
            
            var tokenServices = ResolveTokenServices();
            
            var command = new InitiateStableCoinPurchaseTransactionCommand
            {
                PaymentCurrency = "USDT",
                PaymentProcessor = "KANGA",
                ProjectId = Guid.NewGuid(),
                TokenAmount = 100,
                WalletAddress = EthereumAddressFaker.Generate()
            };
            
            var handler = new InitiateStableCoinPurchaseTransactionCommandHandler(
                eventFactory.Object, 
                eventPublisher.Object, 
                userContext,
                walletDbContext,
                projectPermissionFactory.Object,
                projectManagementClient.Object,
                userWalletService.Object,
                logger.Object);
            
            Assert.ThrowsAsync<UnsupportedCurrencyException>(async () => await handler.Handle(command, CancellationToken.None));
            await Task.CompletedTask;
        }

        [Test]
        public async Task ShouldThrowExceptionWhenInsufficientPaymentCurrencyBalance()
        {
            //Arrange
            var walletDbContext = GetContext<WalletContext>();
            var userContext = CreateCurrentUserContextMock();
            var projectPermissionFactory = new Mock<IProjectPermissionFactory>();
            projectPermissionFactory
                .Setup(x => x.GetUserAbilityToPurchaseAsync(
                    It.IsAny<Guid>(), 
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            walletDbContext.CreateUsdt();
            var token = walletDbContext.CreateToken();
            await walletDbContext.SaveChangesAsync();
            var projectManagementClient = ResolveProjectManagementClientReturningValue(token, true);     
            var projectStage = Builder<ProjectStage>.CreateNew().Build();
            projectStage.TokenPrice = 6;
            projectManagementClient
                .Setup(m => m.CurrentProjectSaleStage(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(projectStage);
            
            var userWalletService = new Mock<IUserWalletService>();
            userWalletService.Setup(x =>
                    x.PaymentCurrencyBalanceAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(99M);
            
            var eventPublisher = new Mock<IEventPublisher>();
            var eventFactory = new Mock<IEventFactory>();
            eventFactory.Setup(m => m.CreateEvent(It.IsAny<string>(), It.IsAny<object>())).Returns(new CloudEvent());
            var logger = new Mock<ILogger>();
            var tokenServices = ResolveTokenServices();
            
            var command = new InitiateStableCoinPurchaseTransactionCommand
            {
                PaymentCurrency = "USDT",
                PaymentProcessor = "KANGA",
                ProjectId = Guid.NewGuid(),
                TokenAmount = 20,
                WalletAddress = EthereumAddressFaker.Generate()
            };
            
            var handler = new InitiateStableCoinPurchaseTransactionCommandHandler(
                eventFactory.Object, 
                eventPublisher.Object, 
                userContext,
                walletDbContext,
                projectPermissionFactory.Object,
                projectManagementClient.Object,
                userWalletService.Object,
                logger.Object);
            
            Assert.ThrowsAsync<InsufficientCurrencyBalanceException>(async () => await handler.Handle(command, CancellationToken.None));
            await Task.CompletedTask;
        }
        
        [Test]
        public async Task ShouldThrowExceptionWhenInsufficientTokensAmount()
        {
            //Arrange
            var walletDbContext = GetContext<WalletContext>();
            var userContext = CreateCurrentUserContextMock();
            var projectPermissionFactory = new Mock<IProjectPermissionFactory>();
            projectPermissionFactory
                .Setup(x => x.GetUserAbilityToPurchaseAsync(
                    It.IsAny<Guid>(), 
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            
            walletDbContext.CreateUsdt();
            var token = walletDbContext.CreateToken();
            await walletDbContext.SaveChangesAsync();
            var projectManagementClient = ResolveProjectManagementClientReturningValue(token, true);         
            var projectStage = Builder<ProjectStage>.CreateNew().Build();
            projectStage.TokenPrice = 1;
            projectManagementClient
                .Setup(m => m.CurrentProjectSaleStage(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(projectStage);
            
            var userWalletService = new Mock<IUserWalletService>();
            userWalletService.Setup(x =>
                    x.PaymentCurrencyBalanceAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(101M);
            var eventPublisher = new Mock<IEventPublisher>();
            var eventFactory = new Mock<IEventFactory>();
            eventFactory.Setup(m => m.CreateEvent(It.IsAny<string>(), It.IsAny<object>())).Returns(new CloudEvent());
            var logger = new Mock<ILogger>();
            var tokenServices = ResolveTokenServices();
            
            var command = new InitiateStableCoinPurchaseTransactionCommand
            {
                PaymentCurrency = "USDT",
                PaymentProcessor = "KANGA",
                ProjectId = Guid.NewGuid(),
                TokenAmount = 100,
                WalletAddress = EthereumAddressFaker.Generate()
            };
            
            var handler = new InitiateStableCoinPurchaseTransactionCommandHandler(
                eventFactory.Object, 
                eventPublisher.Object, 
                userContext,
                walletDbContext,
                projectPermissionFactory.Object,
                projectManagementClient.Object,
                userWalletService.Object,
                logger.Object);
            
            Assert.ThrowsAsync<InsufficientTokensException>(async () => await handler.Handle(command, CancellationToken.None));
            await Task.CompletedTask;
        }
        
        [Test]
        public async Task ShouldSaveTransactionInDatabase()
        {
            //Arrange
            var walletDbContext = GetContext<WalletContext>();
            var userContext = CreateCurrentUserContextMock();
            var projectPermissionFactory = new Mock<IProjectPermissionFactory>();
            projectPermissionFactory
                .Setup(x => x.GetUserAbilityToPurchaseAsync(
                    It.IsAny<Guid>(), 
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var buyerWallet = walletDbContext.CreateWallet(userContext.UserId);
            walletDbContext.CreateUsdt();
            var token = walletDbContext.CreateToken();
            token.TokensLeft = 121;
            await walletDbContext.SaveChangesAsync();
            var projectManagementClient = ResolveProjectManagementClientReturningValue(token, true);   
            var projectStage = Builder<ProjectStage>.CreateNew().Build();
            projectStage.TokenPrice = 2;
            projectManagementClient
                .Setup(m => m.CurrentProjectSaleStage(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(projectStage);
            
            var userWalletService = new Mock<IUserWalletService>();
            userWalletService.Setup(x =>
                    x.PaymentCurrencyBalanceAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(130M);
            var eventPublisher = new Mock<IEventPublisher>();
            var eventFactory = new Mock<IEventFactory>();
            eventFactory.Setup(m => m.CreateEvent(It.IsAny<string>(), It.IsAny<object>())).Returns(new CloudEvent());
            var logger = new Mock<ILogger>();
            var tokenServices = ResolveTokenServices();

            var command = new InitiateStableCoinPurchaseTransactionCommand
            {
                PaymentCurrency = "USDT",
                PaymentProcessor = "KANGA",
                ProjectId = Guid.NewGuid(),
                TokenAmount = 60,
                WalletAddress = buyerWallet.AccountAddress
            };
            
            var handler = new InitiateStableCoinPurchaseTransactionCommandHandler(
                eventFactory.Object, 
                eventPublisher.Object, 
                userContext,
                walletDbContext,
                projectPermissionFactory.Object,
                projectManagementClient.Object,
                userWalletService.Object,
                logger.Object);
            
            await handler.Handle(command, CancellationToken.None);
            
            // Assert
            var createdTransaction = await walletDbContext
                .Transactions
                .AsNoTracking()
                .Where(x => x.TokenAmount == 60 && x.WalletAddress == buyerWallet.AccountAddress)
                .SingleOrDefaultAsync();
            
            Assert.NotNull(createdTransaction);
        }
    }
}