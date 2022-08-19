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
using Mosaico.Events.BusinessManagement;
using Mosaico.Events.Wallet;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.Persistence.SqlServer.Contexts.Wallet;
using Mosaico.SDK.BusinessManagement.Abstractions;
using Mosaico.SDK.BusinessManagement.Models;
using Mosaico.Tests.Base;
using Nethereum.Signer;
using Nethereum.Web3.Accounts;
using NUnit.Framework;
using Serilog;

namespace Mosaico.Application.Wallet.Tests.EventHandlers
{
    // public class CreateWalletOnCompanyCreatedTests : EFInMemoryTestBase
    // {
    //     protected override List<Profile> Profiles { get; }
    //     
    //     protected override void RegisterDependencies(ContainerBuilder builder)
    //     {
    //         base.RegisterDependencies(builder);
    //         RegisterContext<WalletContext>(builder);
    //         builder.RegisterModule(new WalletApplicationModule());
    //     }
    //     
    //     [Test]
    //     public async Task ShouldCreateWalletForCompanyAndSaveInDatabase()
    //     {
    //         //Arrange
    //         var companyId = Guid.NewGuid();
    //         var ethereumClient = new Mock<IEthereumClient>();
    //         var accountMock = new Mock<Account>();
    //         ethereumClient.Setup(m => m.CreateWallet()).Returns(new Account(Guid.NewGuid().ToString(), Chain.Private));
    //         var managementClient = new Mock<IBusinessManagementClient>();
    //         managementClient.Setup(m => m.GetCompanyAsync(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(new MosaicoCompany
    //         {
    //             Id = companyId
    //         });
    //         
    //         var eventPublisher = new Mock<IEventPublisher>();
    //         var eventFactory = new Mock<IEventFactory>();
    //         eventFactory.Setup(m => m.CreateEvent(It.IsAny<string>(), It.IsAny<object>())).Returns(new CloudEvent());
    //         var logger = new Mock<ILogger>();
    //
    //         var ef = new CloudEventFactory();
    //         var eventPayload = new CompanyCreatedEvent(companyId, CurrentUserContext.UserId);
    //         var @event = ef.CreateEvent(Events.BusinessManagement.Constants.EventPaths.Companies, eventPayload);
    //         var walletDbContext = GetContext<IWalletDbContext>();
    //         var handler = new CreateCompanyWalletOnCompanyCreated(managementClient.Object, eventPublisher.Object, eventFactory.Object, walletDbContext, logger.Object);
    //
    //         //Act
    //         await handler.HandleAsync(@event);
    //
    //         var companyWalletEntities = await walletDbContext
    //             .CompanyWallets
    //             .Where(m => m.CompanyId == companyId)
    //             .SingleOrDefaultAsync();
    //         
    //         //Assert
    //         eventFactory.Verify(v => v.CreateEvent(It.Is<string>(m => m == Events.Wallet.Constants.EventPaths.CompanyWallets), It.IsAny<CompanyWalletCreatedEvent>()));
    //         Assert.NotNull(companyWalletEntities);
    //     }
    // }
}