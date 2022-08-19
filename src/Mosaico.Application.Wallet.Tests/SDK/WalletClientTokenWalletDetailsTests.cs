using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using Mosaico.Application.Wallet.Tests.Factories.Domain;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.Persistence.SqlServer.Contexts.Wallet;
using Mosaico.SDK.Wallet;
using Mosaico.Tests.Base;
using NUnit.Framework;

namespace Mosaico.Application.Wallet.Tests.SDK
{
    public class WalletClientTokenWalletDetailsTests : EFInMemoryTestBase
    {
        protected override List<Profile> Profiles => new()
        {
            new WalletMapperProfile(),
            new WalletSDKMapperProfile()
        };
        
        private IMapper _mapper;

        protected override void RegisterDependencies(ContainerBuilder builder)
        {
            base.RegisterDependencies(builder);
            RegisterContext<WalletContext>(builder);
            builder.RegisterModule(new WalletApplicationModule());
            CurrentUserContext = CreateCurrentUserContextMock();
            builder.RegisterInstance(CurrentUserContext).AsImplementedInterfaces();
            
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new WalletMapperProfile());
            });
            _mapper = mockMapper.CreateMapper();
            builder.RegisterInstance(_mapper).AsImplementedInterfaces();
        }

        [Test]
        public async Task ShouldThrowExceptionWhenWalletNotFound()
        {
            //Arrange
            var walletDbContext = GetContext<WalletContext>();
            var sut = new WalletClient(walletDbContext, _mapper);

            //Act & Assert
            Assert.ThrowsAsync<TokenNotFoundException>(async () => await sut.TokenWalletDetails(Guid.NewGuid()));
            await Task.CompletedTask;
        }

        private async Task<Token> FillDatabaseWithTransactions(WalletContext walletDbContext)
        {
            //Arrange
            var userId = CurrentUserContext.UserId;
            walletDbContext.CreateConfirmed();
            var token = walletDbContext.CreateToken();
            
            var transaction1 = walletDbContext.CreateConfirmedPurchaseTransaction(userId, token.Id);
            transaction1.TokenAmount = 90;
            transaction1.LastConfirmationAttemptedAt = DateTimeOffset.Now.AddHours(-6);
            
            var transaction2 = walletDbContext.CreateConfirmedPurchaseTransaction(userId, token.Id);
            transaction2.TokenAmount = 70;
            transaction2.LastConfirmationAttemptedAt = DateTimeOffset.Now.AddHours(-4);
            
            var transaction3 = walletDbContext.CreateConfirmedPurchaseTransaction(userId, token.Id);
            transaction3.TokenAmount = 50;
            transaction3.LastConfirmationAttemptedAt = DateTimeOffset.Now.AddHours(-3);
            
            var transaction4 = walletDbContext.CreateConfirmedPurchaseTransaction(userId, token.Id);
            transaction4.TokenAmount = 45;
            transaction4.LastConfirmationAttemptedAt = DateTimeOffset.Now.AddHours(-2);
            
            await walletDbContext.SaveChangesAsync();
            
            return token;
        }

        [Test]
        public async Task ShouldReturnTokensOfAllTransactions()
        {
            //Arrange
            var walletDbContext = GetContext<WalletContext>();
            var token = await FillDatabaseWithTransactions(walletDbContext);
           
            var sut = new WalletClient(walletDbContext, _mapper);

            //Act
            var resultWithoutDates = await sut.TokenWalletDetails(token.Id);
            
            //Assert
            Assert.AreEqual(255, resultWithoutDates.SoldTokensAmount);
        }

        [Test]
        public async Task ShouldReturnTokensOfTransactionsAfterDate()
        {
            //Arrange
            var walletDbContext = GetContext<WalletContext>();
            var token = await FillDatabaseWithTransactions(walletDbContext);
           
            var sut = new WalletClient(walletDbContext, _mapper);

            //Act
            var resultWithFromDate = await sut.TokenWalletDetails(token.Id, DateTimeOffset.UtcNow.AddMinutes(-121));
            
            //Assert
            Assert.AreEqual(45, resultWithFromDate.SoldTokensAmount);
        }

        [Test]
        public async Task ShouldReturnTokensOfTransactionsBeforeDate()
        {
            //Arrange
            var walletDbContext = GetContext<WalletContext>();
            var token = await FillDatabaseWithTransactions(walletDbContext);
           
            var sut = new WalletClient(walletDbContext, _mapper);

            //Act
            var resultWithFromDate = await sut.TokenWalletDetails(token.Id, DateTimeOffset.UtcNow.AddMinutes(-241), DateTimeOffset.UtcNow.AddMinutes(-121));
            
            //Assert
            Assert.AreEqual(120, resultWithFromDate.SoldTokensAmount);
        }
        
    }
}