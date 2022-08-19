using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using FizzWare.NBuilder;
using KangaExchange.SDK.Abstractions;
using KangaExchange.SDK.Configurations;
using KangaExchange.SDK.Models.BuyClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Mosaico.Application.KangaWallet.Commands.CreateTransaction;
using Mosaico.Application.ProjectManagement.Permissions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Events.Base;
using Mosaico.Persistence.SqlServer.Contexts.Wallet;
using Mosaico.SDK.Identity.Abstractions;
using Mosaico.SDK.ProjectManagement.Abstractions;
using Mosaico.SDK.ProjectManagement.Models;
using Mosaico.Tests.Base;
using NUnit.Framework;
using Serilog;

namespace Mosaico.Application.Wallet.Tests.Commands
{
    public class CreateTransactionCommandHandlerTests : EFInMemoryTestBase
    {
        protected override List<Profile> Profiles => new()
        {
            new WalletMapperProfile()
        };

        private Mock<ILogger> _logger;

        protected override void RegisterDependencies(ContainerBuilder builder)
        {
            base.RegisterDependencies(builder);
            RegisterContext<WalletContext>(builder);
            builder.RegisterModule(new WalletApplicationModule());
            
            _logger = new Mock<ILogger>();
            builder.RegisterInstance(_logger.Object).AsImplementedInterfaces();
            CurrentUserContext = CreateCurrentUserContextMock();
        }
        
        [Test]
        public async Task ShouldCreateAndSaveTransaction()
        {
            //Arrange
            var userId = CurrentUserContext.UserId;
            var walletDbContext = GetContext<WalletContext>();
            var eventPublisher = new Mock<IEventPublisher>();
            var eventFactory = new Mock<IEventFactory>();
            eventFactory.Setup(m => m.CreateEvent(It.IsAny<string>(), It.IsAny<object>())).Returns(new CloudEvent());
            var userManagementClient = new Mock<IUserManagementClient>();
            userManagementClient.Setup(x => x.CreateKangaUserIfNotExist(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            
            var token = Builder<Token>.CreateNew().Build();
            token.Id = Guid.NewGuid();
            walletDbContext.Tokens.Add(token);
            var wallet = Builder<CompanyWallet>.CreateNew().Build();
            wallet.Id = Guid.NewGuid();
            wallet.CompanyId = Guid.Parse("0b214de7-8958-4956-8eed-28f9ba2c47c6");
            wallet.Network = Blockchain.Base.Constants.BlockchainNetworks.Ethereum;
            walletDbContext.CompanyWallets.Add(wallet);
            
            await walletDbContext.SaveChangesAsync();

            var projectManagementClient = new Mock<IProjectManagementClient>();
            var projectDetails = Builder<MosaicoProjectDetails>.CreateNew().Build();
            projectDetails.TokenId = token.Id;
            projectDetails.PaymentMethods = new List<string> {"KANGA_EXCHANGE"};
            projectDetails.SaleInProgress = true;
            projectManagementClient.Setup(x => x.GetProjectDetailsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(projectDetails);
            
            var permissionFactory = new Mock<IProjectPermissionFactory>();
            permissionFactory.Setup(x => x.GetUserAbilityToPurchaseAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            
            var kangaBuyApiClient = new Mock<IKangaBuyApiClient>();
            kangaBuyApiClient.Setup(x => x.BuyAsync(It.IsAny<Action<BuyParams>>()))
                .ReturnsAsync(new BuyResponseDto
                {
                    OrderId = Guid.NewGuid().ToString(),
                    RedirectUrl = "http://kanga.exchange",
                    Result = "ok"
                });
            
            var command = new CreateTransactionCommand
            {
                CurrencyAmount = 40,
                TokenAmount = 20,
                PaymentCurrency = "oPLN",
                ProjectId = Guid.NewGuid().ToString(),
                UserId = userId
            };
            
            var handler = new CreateTransactionCommandHandler(
                kangaBuyApiClient.Object, 
                userManagementClient.Object,
                projectManagementClient.Object,
                permissionFactory.Object,
                CurrentUserContext,
                walletDbContext,
                eventPublisher.Object,
                eventFactory.Object,
                new KangaConfiguration
                {
                    AfterPurchaseRedirectUrl = "http://localhost:4200/test-confirmation"
                },
                null,
                _logger.Object);

            var result = await handler.Handle(command, CancellationToken.None);
            var processedTransaction = await walletDbContext
                .Transactions
                .FirstOrDefaultAsync();
            
            Assert.NotNull(processedTransaction);
            Assert.AreEqual(processedTransaction.PayedAmount, 40);
            Assert.AreEqual(result.KangaAccountCreated, true);
        }
    }
}