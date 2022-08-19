// using System.Collections.Generic;
// using System.Numerics;
// using System.Threading.Tasks;
// using Mosaico.Integration.Blockchain.Ethereum.Configuration;
// using Mosaico.Integration.Blockchain.Ethereum.DefaultCrowdsalev1;
// using Mosaico.Integration.Blockchain.Ethereum.DefaultCrowdsalev1.ContractDefinition;
// using Mosaico.Integration.Blockchain.Ethereum.MosaicoERC20v1;
// using Mosaico.Integration.Blockchain.Ethereum.MosaicoERC20v1.ContractDefinition;
// using Mosaico.Integration.Blockchain.Ethereum.Tests.Configuration;
// using Mosaico.Integration.Blockchain.Ethereum.Tests.Helpers;
// using Mosaico.Tests.Base;
// using Nethereum.Web3;
// using Nethereum.Web3.Accounts;
// using NUnit.Framework;
//
// namespace Mosaico.Integration.Blockchain.Ethereum.Tests.Crowdsale
// {
//     public class CrowdsaleTests : TestBase
//     {
//         private EthereumNetworkConfiguration _config;
//         private EthereumTestConfiguration _testConfig;
//         
//         [SetUp]
//         public void Setup()
//         {
//             _config = GetSettings<EthereumNetworkConfiguration>(EthereumNetworkConfiguration.SectionName);
//             _testConfig = GetSettings<EthereumTestConfiguration>(EthereumTestConfiguration.SectionName);
//         }
//         
//         [Test]
//         public async Task ShouldDeployCrowdSaleContract()
//         {
//             //Arrange
//             var rates = new List<decimal> {2};
//             var caps = new List<long> {1000};
//             var privateSales = new List<bool> {false};
//             var names = new List<string> {"Sale"};
//             
//             var adminProvider = new EthereumAdminAccountConfigurationProvider(_config);
//             var client = new EthereumClient(adminProvider, _config, null);
//             var serviceFactory = new EthereumServiceFactory(adminProvider, _config, null);
//             //Act
//             var tokenAddress = await client.DeployERC20();
//             Assert.IsNotEmpty(tokenAddress);
//             var settings = DefaultCrowdsalev1Settings.GetSettings(_testConfig.AdminWalletAddress, tokenAddress, rates.Count);
//             var crowdSaleAddress = await client.DeployContractAsync<DefaultCrowdsalev1Deployment>(null, settings);
//             var crowdSaleContract = await serviceFactory.GetServiceAsync<DefaultCrowdsalev1Service>(crowdSaleAddress);
//             var stageSettings = DefaultCrowdsalev1Settings.GetStartNextStageFunction(names[0], caps[0], rates[0], privateSales[0], 100, 1000, new List<string>());
//             //Assert
//             Assert.IsNotEmpty(crowdSaleAddress);
//             await crowdSaleContract.StartNextStageRequestAndWaitForReceiptAsync(stageSettings);
//             Assert.NotNull(crowdSaleContract);
//             
//             var rateCallFunctionResult = await crowdSaleContract.GetRateQueryAsync();
//             Assert.NotNull(rateCallFunctionResult);
//             Assert.AreEqual(new BigInteger(rates[0]), rateCallFunctionResult);
//         }
//         
//         [Test]
//         public async Task ShouldTransferTokensToCrowdSaleAddress()
//         {
//             //Arrange
//             var rates = new List<decimal> {2};
//             
//             var adminProvider = new EthereumAdminAccountConfigurationProvider(_config);
//             var client = new EthereumClient(adminProvider, _config, null);
//             var serviceFactory = new EthereumServiceFactory(adminProvider, _config, null);
//             
//             //Act
//             var tokenAddress = await client.DeployERC20();
//             Assert.IsNotEmpty(tokenAddress);
//             var settings = DefaultCrowdsalev1Settings.GetSettings(_testConfig.AdminWalletAddress, tokenAddress, rates.Count);
//             var crowdSaleAddress = await client.DeployContractAsync<DefaultCrowdsalev1Deployment>(null, settings);
//             var tokenContract = await serviceFactory.GetServiceAsync<MosaicoERC20v1Service>(tokenAddress);
//             await tokenContract.TransferRequestAsync(crowdSaleAddress, ERC20Deployer.InitialSupply);
//             var crowdSaleTokenBalance = await tokenContract.BalanceOfQueryAsync(crowdSaleAddress);
//
//             //Assert
//             Assert.NotNull(tokenContract);
//             Assert.AreEqual(new BigInteger(ERC20Deployer.InitialSupply), crowdSaleTokenBalance);
//         }
//
//         [Test]
//         public async Task ShouldTransferFundsToAddressWhoBoughtTokens()
//         {
//             //Arrange
//             var rates = new List<decimal> {2};
//             var caps = new List<long> {1000};
//             var privateSales = new List<bool> {false};
//             var names = new List<string> {"Sale"};
//             
//             var adminProvider = new EthereumAdminAccountConfigurationProvider(_config);
//             var client = new EthereumClient(adminProvider, _config, null);
//             var serviceFactory = new EthereumServiceFactory(adminProvider, _config, null);
//
//             //Act
//             var tokenAddress = await client.DeployERC20();
//             Assert.IsNotEmpty(tokenAddress);
//             var settings = DefaultCrowdsalev1Settings.GetSettings(_testConfig.AdminWalletAddress, tokenAddress, rates.Count);
//             var crowdSaleAddress = await client.DeployContractAsync<DefaultCrowdsalev1Deployment>(null, settings);
//             var tokenContract = await serviceFactory.GetServiceAsync<MosaicoERC20v1Service>(tokenAddress);
//             await tokenContract.TransferRequestAsync(crowdSaleAddress, ERC20Deployer.InitialSupply);
//             var account = new Account(_config.AdminAccount.PrivateKey);
//             var b = await client.GetAccountBalanceAsync(account.Address);
//             var crowdSaleContract = await serviceFactory.GetServiceAsync<DefaultCrowdsalev1Service>(crowdSaleAddress);
//             var stageSettings = DefaultCrowdsalev1Settings.GetStartNextStageFunction(names[0], caps[0], rates[0], privateSales[0], 100, 1000, new List<string>());
//             await crowdSaleContract.StartNextStageRequestAndWaitForReceiptAsync(stageSettings);
//             var web3 = client.GetClient(account);
//             var l = await web3.Eth.GetEtherTransferService()
//                 .TransferEtherAndWaitForReceiptAsync(crowdSaleAddress, Web3.Convert.FromWei(101), null, new BigInteger(183000));
//             
//             var crowdSaleTokenBalance = await tokenContract.BalanceOfQueryAsync(_testConfig.AdminWalletAddress);
//             
//             //Assert
//             Assert.NotNull(tokenContract);
//             Assert.AreEqual(new BigInteger(202), crowdSaleTokenBalance);
//         }
//     }
// }