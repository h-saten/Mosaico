// using System;
// using System.Collections.Generic;
// using System.Numerics;
// using System.Threading.Tasks;
// using Moq;
// using Mosaico.Integration.Blockchain.Ethereum.Configuration;
// using Mosaico.Integration.Blockchain.Ethereum.DefaultCrowdsalev1;
// using Mosaico.Integration.Blockchain.Ethereum.DefaultCrowdsalev1.ContractDefinition;
// using Mosaico.Integration.Blockchain.Ethereum.MosaicoERC20v1;
// using Mosaico.Integration.Blockchain.Ethereum.Services.v1;
// using Mosaico.Integration.Blockchain.Ethereum.Tests.Configuration;
// using Mosaico.Integration.Blockchain.Ethereum.Tests.Helpers;
// using Mosaico.Tests.Base;
// using NUnit.Framework;
// using Serilog;
//
// namespace Mosaico.Integration.Blockchain.Ethereum.Tests.Crowdsale
// {
//     public class CrowdsaleServiceTests : TestBase
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
//         public async Task ShouldBuyTokensByExchangingEtherToValidTokensAmount()
//         {
//             //arrange
//             var adminProvider = new EthereumAdminAccountConfigurationProvider(_config);
//             var client = new EthereumClient(adminProvider, _config, null);
//             var ethServiceFactory = new EthereumServiceFactory(adminProvider, _config);
//             var crowdSaleService = new CrowdsaleService(ethServiceFactory, client);
//             var serviceFactory = new EthereumServiceFactory(adminProvider, _config, null);
//
//             var tokenSupply = 1000000000000000000;
//             var tokenAddress = await client.DeployERC20(null, null, tokenSupply);
//             Assert.IsNotEmpty(tokenAddress);
//             var tokenContract = await serviceFactory.GetServiceAsync<MosaicoERC20v1Service>(tokenAddress);
//
//             var rates = new List<decimal> {2};
//             var caps = new List<long> {100000000000000000};
//             var privateSales = new List<bool> {false};
//             var names = new List<string> {"Sale"};
//             var settings = DefaultCrowdsalev1Settings.GetSettings(_testConfig.AdminWalletAddress, tokenAddress, rates.Count);
//             var crowdSaleAddress = await client.DeployContractAsync<DefaultCrowdsalev1Deployment>(null, settings);
//             Assert.IsNotEmpty(crowdSaleAddress);
//             var crowdSaleContract = await serviceFactory.GetServiceAsync<DefaultCrowdsalev1Service>(crowdSaleAddress);
//             await tokenContract.TransferRequestAsync(crowdSaleAddress, tokenSupply);
//             var stageSettings = DefaultCrowdsalev1Settings.GetStartNextStageFunction(names[0], caps[0], rates[0], privateSales[0], 100, 100000000000000000, new List<string>());
//             await crowdSaleContract.StartNextStageRequestAndWaitForReceiptAsync(stageSettings);
//
//             var amount = Decimal.Parse("0.000000005"); // -> 10000000000
//             var userAccount = client.CreateWallet();
//
//             await crowdSaleService.BuyTokens(crowdSaleAddress, c =>
//             {
//                 c.Amount = amount;
//                 c.Beneficiary = userAccount.Address;
//             });
//
//             var userBalance = await tokenContract.BalanceOfQueryAsync(userAccount.Address);
//             Assert.AreEqual((BigInteger) 10000000000, userBalance);
//         }
//     }
// }