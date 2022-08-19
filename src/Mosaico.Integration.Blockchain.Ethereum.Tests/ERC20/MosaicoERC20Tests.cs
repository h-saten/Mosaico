// using System.Numerics;
// using System.Threading.Tasks;
// using Mosaico.Integration.Blockchain.Ethereum.Configuration;
// using Mosaico.Integration.Blockchain.Ethereum.MosaicoERC20v1;
// using Mosaico.Integration.Blockchain.Ethereum.Tests.Helpers;
// using Mosaico.Tests.Base;
// using NUnit.Framework;
//
// namespace Mosaico.Integration.Blockchain.Ethereum.Tests.ERC20
// {
//     public class MosaicoERC20Tests : TestBase
//     {
//         private EthereumNetworkConfiguration _config;
//         
//         [SetUp]
//         public void Setup()
//         {
//             _config = GetSettings<EthereumNetworkConfiguration>(EthereumNetworkConfiguration.SectionName);
//         }
//         
//         [Test]
//         public async Task ShouldDeployERC20Contract()
//         {
//             //Arrange
//             
//             var adminProvider = new EthereumAdminAccountConfigurationProvider(_config);
//             var client = new EthereumClient(adminProvider, _config, null);
//             var serviceFactory = new EthereumServiceFactory(adminProvider, _config, null);
//             //Act
//             var address = await client.DeployERC20();
//             
//             //Assert
//             Assert.IsNotEmpty(address);
//             var contract = await serviceFactory.GetServiceAsync<MosaicoERC20v1Service>(address);
//             Assert.NotNull(contract);
//             
//             var symbolCallFunctionResult = await contract.SymbolQueryAsync();
//             Assert.NotNull(symbolCallFunctionResult);
//             Assert.AreEqual(ERC20Deployer.TokenSymbol, symbolCallFunctionResult);
//             
//             var totalSupplyFunctionResult = await contract.TotalSupplyQueryAsync();
//             Assert.AreEqual(new BigInteger(ERC20Deployer.InitialSupply), totalSupplyFunctionResult);
//         }
//     }
// }