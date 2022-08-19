using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using FizzWare.NBuilder;
using Microsoft.EntityFrameworkCore;
using Moq;
using Mosaico.Application.Wallet.Services;
using Mosaico.Application.Wallet.Tests.Factories.Domain;
using Mosaico.Application.Wallet.Tests.Factories.Events;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.Persistence.SqlServer.Contexts.Wallet;
using Mosaico.SDK.ProjectManagement.Abstractions;
using Mosaico.SDK.ProjectManagement.Models;
using Mosaico.Tests.Base;
using NUnit.Framework;

namespace Mosaico.Application.Wallet.Tests.Services
{
    // public class BuyTokenServiceTests : EFInMemoryTestBase
    // {
    //     protected override List<Profile> Profiles { get; }
    //     private IProjectManagementClient _projectManagementClient;
    //     private ICrowdsaleService _crowdSaleService;
    //
    //     protected override void RegisterDependencies(ContainerBuilder builder)
    //     {
    //         base.RegisterDependencies(builder);
    //         RegisterContext<WalletContext>(builder);
    //         builder.RegisterModule(new WalletApplicationModule());
    //         
    //         var projectManagementClient = new Mock<IProjectManagementClient>();
    //         projectManagementClient
    //             .Setup(m => m.GetProjectDetailsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
    //             .ReturnsAsync(Builder<MosaicoProjectDetails>.CreateNew().Build());
    //         projectManagementClient
    //             .Setup(m => m.CurrentProjectSaleStage(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
    //             .ReturnsAsync(Builder<ProjectStage>.CreateNew().Build());
    //         _projectManagementClient = projectManagementClient.Object;
    //         builder.RegisterInstance(_projectManagementClient).AsImplementedInterfaces();
    //         
    //         var crowdSaleService = new Mock<ICrowdsaleService>();
    //         crowdSaleService.Setup(m => m.BuyTokens(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Action<BuyTokensConfiguration>>()))
    //             .Returns(Task.CompletedTask);
    //         _crowdSaleService = crowdSaleService.Object;
    //         builder.RegisterInstance(_crowdSaleService).AsImplementedInterfaces();
    //
    //         CurrentUserContext = CreateCurrentUserContextMock();
    //     }
    //     
    //     [Test]
    //     public async Task ShouldSaveFailureReasonWhenTokenNotExist()
    //     {
    //         //Arrange
    //         var walletDbContext = GetContext<IWalletDbContext>();
    //         var transactionStatusPending = walletDbContext.CreatePending();
    //         var userId = CurrentUserContext.UserId;
    //         var transactionType = walletDbContext.CreatePurchase();
    //         var transaction = walletDbContext.CreateTransaction(userId, Guid.NewGuid(), transactionType, transactionStatusPending);
    //
    //         await walletDbContext.SaveChangesAsync();
    //         
    //         var eventPayload = TransactionInitiatedEventFactory.Create(transaction.Id);
    //         var sut = new BuyTokenService(walletDbContext, _projectManagementClient, _crowdSaleService);
    //
    //         //Act
    //         await sut.BuyAsync(eventPayload, CancellationToken.None);
    //         
    //         //Assert
    //         var transactionResult = await walletDbContext
    //             .Transactions
    //             .Where(m => m.Id == transaction.Id)
    //             .SingleOrDefaultAsync();
    //         
    //         Assert.IsNotNull(transactionResult);
    //         Assert.IsNotEmpty(transactionResult.FailureReason);
    //         StringAssert.Contains("Token", transactionResult.FailureReason);
    //     }
    //     
    //     [Test]
    //     public async Task ShouldSaveFailureReasonWhenWalletNotExist()
    //     {
    //         //Arrange
    //         var walletDbContext = GetContext<IWalletDbContext>();
    //         var userId = CurrentUserContext.UserId;
    //
    //         var token = walletDbContext.CreateToken();
    //         var transactionStatusPending = walletDbContext.CreatePending();
    //         walletDbContext.CreateConfirmed();
    //         var transactionType = walletDbContext.CreatePurchase();
    //         var transaction = walletDbContext.CreateTransaction(userId, token.Id, transactionType, transactionStatusPending);
    //         await walletDbContext.SaveChangesAsync();
    //     
    //         var eventPayload = TransactionInitiatedEventFactory.Create(transaction.Id);
    //         var sut = new BuyTokenService(walletDbContext, _projectManagementClient, _crowdSaleService);
    //     
    //         //Act
    //         await sut.BuyAsync(eventPayload, CancellationToken.None);
    //         
    //         //Assert
    //         var transactionResult = await walletDbContext
    //             .Transactions
    //             .Where(m => m.Id == transaction.Id)
    //             .SingleOrDefaultAsync();
    //         
    //         Assert.IsNotNull(transactionResult);
    //         Assert.IsNotEmpty(transactionResult.FailureReason);
    //         StringAssert.Contains("Wallet", transactionResult.FailureReason);
    //     }
    //     
    //     
    //     [Test]
    //     public async Task ShouldCreateAndConfirmTransaction()
    //     {
    //         //Arrange
    //         var walletDbContext = GetContext<IWalletDbContext>();
    //         var userId = CurrentUserContext.UserId;
    //
    //         var token = walletDbContext.CreateToken();
    //         var transactionStatusPending = walletDbContext.CreatePending();
    //         walletDbContext.CreateConfirmed();
    //         var transactionType = walletDbContext.CreatePurchase();
    //         var transaction = walletDbContext.CreateTransaction(userId, token.Id, transactionType, transactionStatusPending);
    //         walletDbContext.CreateWallet(userId);
    //         await walletDbContext.SaveChangesAsync();
    //     
    //         var eventPayload = TransactionInitiatedEventFactory.Create(transaction.Id);
    //         var sut = new BuyTokenService(walletDbContext, _projectManagementClient, _crowdSaleService);
    //     
    //         //Act
    //         await sut.BuyAsync(transaction.Id, CancellationToken.None);
    //     
    //         //Assert
    //         var updatedTransaction = await walletDbContext
    //             .Transactions
    //             .Where(m => m.Id == transaction.Id)
    //             .SingleOrDefaultAsync();
    //     
    //         Assert.NotNull(updatedTransaction);
    //         Assert.NotNull(updatedTransaction.Status.Key == Domain.Wallet.Constants.TransactionStatuses.Confirmed);
    //     }
    // }
}