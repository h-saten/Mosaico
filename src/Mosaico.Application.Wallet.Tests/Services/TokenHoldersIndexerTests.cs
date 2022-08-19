using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using FizzWare.NBuilder;
using Microsoft.EntityFrameworkCore;
using Moq;
using Mosaico.Application.Wallet.Services;
using Mosaico.Application.Wallet.Tests.Factories.Domain;
using Mosaico.Blockchain.Base.DAL;
using Mosaico.Blockchain.Base.DAL.Models;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.Persistence.SqlServer.Contexts.Wallet;
using Mosaico.Tests.Base;
using NUnit.Framework;

namespace Mosaico.Application.Wallet.Tests.Services
{
    public class TokenHoldersIndexerTests : EFInMemoryTestBase
    {
        protected override List<Profile> Profiles { get; }
        private ITokenRepository _tokenRepository;
        private IWalletDbContext _walletDbContext;
        private IEthereumClient _ethereumClient;
        private IEthereumClientFactory _ethClientFactory;

        protected override void RegisterDependencies(ContainerBuilder builder)
        {
            base.RegisterDependencies(builder);
            _walletDbContext = RegisterContext<WalletContext>(builder);
            builder.RegisterModule(new WalletApplicationModule());

            var transfer1 = new Builder().CreateNew<ERC20Transfer>().Build();
            transfer1.Value = 5;
            transfer1.FromAddress = "0x38b44dc7141A4C18D8a05A418F4e05A874d64Dcf";
            transfer1.ToAddress = "0xbc90C1e49c09C714519BF61E27C204801343d92f";

            var transfer2 = new Builder().CreateNew<ERC20Transfer>().Build();
            transfer2.Value = 15;
            transfer2.FromAddress = "0x38b44dc7141A4C18D8a05A418F4e05A874d64Dcf";
            transfer2.ToAddress = "0xbc90C1e49c09C714519BF61E27C204801343d92f";
            
            var transfer3 = new Builder().CreateNew<ERC20Transfer>().Build();
            transfer3.Value = 7;
            transfer3.FromAddress = "0xbc90C1e49c09C714519BF61E27C204801343d92f";
            transfer3.ToAddress = "0x38b44dc7141A4C18D8a05A418F4e05A874d64Dcf";

            var transfers = new List<ERC20Transfer> { transfer1, transfer2, transfer3 };
            
            var tokenRepository = new Mock<ITokenRepository>();
            tokenRepository.Setup(x =>
                    x.Erc20TransfersAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<ulong?>(), It.IsAny<ulong?>()))
                .ReturnsAsync(transfers);
            _tokenRepository = tokenRepository.Object;            
            
            var ethereumClient = new Mock<IEthereumClient>();
            ethereumClient.Setup(x =>
                    x.LatestBlockNumberAsync())
                    .ReturnsAsync(1000000);
            _ethereumClient = ethereumClient.Object;

            CurrentUserContext = CreateCurrentUserContextMock();
        }

        [Test]
        public async Task TestWithMockedData()
        {
            //Arrange
            var walletDbContext = GetContext<IWalletDbContext>();

            var token = walletDbContext.CreateToken();
            await walletDbContext.SaveChangesAsync();

            var sut = new TokenHoldersIndexer(_walletDbContext, _tokenRepository, _ethClientFactory);

            var result = await sut.UpdateHoldersAsync(token.Id);

            var holdersAmount = await walletDbContext
                .TokenHolders
                .CountAsync();
            
            Assert.AreEqual(2, holdersAmount);
            Assert.AreEqual(13m, result["0xbc90C1e49c09C714519BF61E27C204801343d92f"]);
            Assert.AreEqual(-13m, result["0x38b44dc7141A4C18D8a05A418F4e05A874d64Dcf"]);
        }
    }
}