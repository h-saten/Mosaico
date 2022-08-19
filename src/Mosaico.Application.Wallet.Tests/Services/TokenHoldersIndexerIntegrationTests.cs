using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using Moq;
using Mosaico.Application.Wallet.Services;
using Mosaico.Application.Wallet.Tests.Factories.Domain;
using Mosaico.Blockchain.Base.DAL;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Integration.Blockchain.Ethereum;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.Integration.Blockchain.Ethereum.Configuration;
using Mosaico.Integration.Blockchain.Ethereum.DAL;
using Mosaico.Persistence.SqlServer.Contexts.Wallet;
using Mosaico.Tests.Base;
using Nethereum.RPC.Eth.DTOs;
using NUnit.Framework;

namespace Mosaico.Application.Wallet.Tests.Services
{
    public class TokenHoldersIndexerIntegrationTests : EFInMemoryTestBase
    {
        protected override List<Profile> Profiles { get; }
        private IWalletDbContext _walletDbContext;
        private ITokenRepository _tokenRepository;
        private IEthereumClient _ethClient;
        private IEthereumClientFactory _ethClientFactory;

        protected override void RegisterDependencies(ContainerBuilder builder)
        {
            base.RegisterDependencies(builder);
            _walletDbContext = RegisterContext<WalletContext>(builder);
            builder.RegisterModule(new WalletApplicationModule());
            CurrentUserContext = CreateCurrentUserContextMock();
            
            

            var ethConfiguration = new EthereumNetworkConfiguration
            {
                Name = "Polygon",
                IsDefault = true,
                LogoUrl = "/assets/media/svg/crypto/matic.svg",
                Endpoint = "https://polygon-mumbai.infura.io/v3/454e8fcf8056410dad961727f75284dd",
                AdminAccount = new EthereumAdminAccount
                {
                    Mnemonic = "horn hammer original lemon chapter weird gun pond fortune blush cupboard dog",
                    Password = "",
                    AccountNumber = "0x7b9b4f50d30f8eba180081741226ce1deee15c8b894af500106bd36e9123a22d",
                    PrivateKey = "0x7b9b4f50d30f8eba180081741226ce1deee15c8b894af500106bd36e9123a22d"
                },
                Chain = "137",
                AdminAccountProviderType = "CONFIGURATION",
                EtherscanApiToken = "",
                EtherscanUrl = "https://polygonscan.com/",
                EtherscanApiUrl = "https://api.polygonscan.com/api",
                BlockTime = 2.3
            };
            var ethClient = new EthereumClient();
            ethClient.Configuration = ethConfiguration;
            _ethClient = ethClient;
            
            var adminAccountProvider = new Mock<IAdminAccountProvider<EthereumAdminAccount>>();
            adminAccountProvider.Setup(x => x.Configuration)
                .Returns(ethConfiguration);
            adminAccountProvider.Setup(x => x.GetAdminAccountDetailsAsync())
                .ReturnsAsync(ethConfiguration.AdminAccount);
            
            ethClient.AdminAccountProvider = adminAccountProvider.Object;

            var ethClientFactory = new Mock<IEthereumClientFactory>();
            ethClientFactory.Setup(x => x.GetClient(It.IsAny<string>()))
                .Returns(ethClient);
            _ethClientFactory = ethClientFactory.Object;

            var blockWithTransactionRepository = new Mock<IBlockWithTransactionRepository>();
            blockWithTransactionRepository
                .Setup(x => x.AddAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>()))
                .Returns(Task.CompletedTask);
            blockWithTransactionRepository
                .Setup(x => x.GetAsync<BlockWithTransactions>(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((BlockWithTransactions) null);
            
            _tokenRepository = new TokenRepository(ethClientFactory.Object, blockWithTransactionRepository.Object);
        }

        [Test]
        public async Task ShouldReturnEmptyResultWhenIndexNewTokenWithoutTransactions()
        {
            //Arrange
            var walletDbContext = GetContext<IWalletDbContext>();

            var token = walletDbContext.CreateToken();
            token.Address = "0x05F19DCCaE366F661E85C40b84Fae0b86876F789";
            await walletDbContext.SaveChangesAsync();
            
            var sut = new TokenHoldersIndexer(_walletDbContext, _tokenRepository, _ethClientFactory);

            var result = await sut.UpdateHoldersAsync(token.Id);

            Assert.AreEqual(0, result.Count);
        }
    }
}