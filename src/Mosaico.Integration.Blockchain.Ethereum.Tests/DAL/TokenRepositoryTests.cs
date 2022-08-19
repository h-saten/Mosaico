using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Moq;
using Mosaico.Blockchain.Base.DAL;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.Integration.Blockchain.Ethereum.Configuration;
using Mosaico.Integration.Blockchain.Ethereum.DAL;
using Mosaico.Integration.Blockchain.Ethereum.MosaicoERC20v1;
using Mosaico.Integration.Blockchain.Ethereum.Tests.Helpers;
using Mosaico.Tests.Base;
using Nethereum.Web3.Accounts;
using NUnit.Framework;

namespace Mosaico.Integration.Blockchain.Ethereum.Tests.DAL
{
    public class TokenRepositoryTests : TestBase
    {
        private BlockchainConfiguration _config;
        
        [SetUp]
        public void Setup()
        {
            _config = GetSettings<BlockchainConfiguration>(BlockchainConfiguration.SectionName);
        }
        
        [Test]
        public async Task ShouldReturn2EventsAfterMintAndTransfer()
        {
            var config = _config.Networks.FirstOrDefault();
            IAdminAccountProvider<EthereumAdminAccount> adminProvider = new EthereumAdminAccountConfigurationProvider
            {
                Configuration = config
            };
            IEthereumClient client = new EthereumClient(null)
            {
                Configuration = config,
                AdminAccountProvider = adminProvider
            };
            IEthereumServiceFactory serviceFactory = new EthereumServiceFactory(null)
            {
                Configuration = config,
                AdminAccountProvider = adminProvider
            };
            var adminAccount = new Account(config.AdminAccount.PrivateKey);
            var indexMock = new Mock<IIndex<string, IEthereumClient>>();
            indexMock.Setup(i => i.TryGetValue(It.IsAny<string>(), out client)).Returns(true);
            var serviceMock = new Mock<IIndex<string, IEthereumServiceFactory>>();
            serviceMock.Setup(i => i.TryGetValue(It.IsAny<string>(), out serviceFactory)).Returns(true);
            var adminAccountMock = new Mock<IIndex<string, IAdminAccountProvider<EthereumAdminAccount>>>();
            adminAccountMock.Setup(i => i.TryGetValue(It.IsAny<string>(), out adminProvider)).Returns(true);
            var configMock = new Mock<IIndex<string, EthereumNetworkConfiguration>>();
            configMock.Setup(i => i.TryGetValue(It.IsAny<string>(), out config)).Returns(true);
            var factory = new EthereumClientFactory(indexMock.Object, serviceMock.Object, adminAccountMock.Object,
                configMock.Object);
            var address = await client.DeployERC20();
            Assert.IsNotEmpty(address);
            var contract = await serviceFactory.GetServiceAsync<MosaicoERC20v1Service>(address, string.Empty);
            await contract.TransferRequestAndWaitForReceiptAsync("0x724C4462912ce12e2ec6434E5Be3aFD7354400a7", new BigInteger(1000));
            
            var apiClient = new TokenRepository(factory, new Mock<IBlockWithTransactionRepository>().Object);
            var events = await apiClient.Erc20TransfersAsync(address, "Ethereum");
            Assert.AreEqual(2, events.Count);
        }
    }
}