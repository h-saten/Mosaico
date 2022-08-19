// using System.Threading.Tasks;
// using Mosaico.Integration.Blockchain.Ethereum.Configuration;
// using Mosaico.Integration.Blockchain.Ethereum.Tests.Configuration;
// using Mosaico.Integration.Blockchain.Ethereum.Tests.Contracts.SimpleStorage;
// using Mosaico.Integration.Blockchain.Ethereum.Tests.Contracts.SimpleStorage.ContractDefinition;
// using Mosaico.Tests.Base;
// using Nethereum.Contracts;
// using NUnit.Framework;
//
// namespace Mosaico.Integration.Blockchain.Ethereum.Tests
// {
//     public class EthereumClientTests : TestBase
//     {
//         private EthereumNetworkConfiguration _config;
//         private EthereumTestConfiguration _testConfiguration;
//         
//         [SetUp]
//         public void Setup()
//         {
//             _config = GetSettings<EthereumNetworkConfiguration>(EthereumNetworkConfiguration.SectionName);
//             _testConfiguration = GetSettings<EthereumTestConfiguration>(EthereumTestConfiguration.SectionName);
//         }
//
//         [Test]
//         public async Task ShouldReturnAccountBalance()
//         {
//             //Arrange
//             var client = new EthereumClient(new EthereumAdminAccountConfigurationProvider(_config), _config, null);
//             var address = _testConfiguration.BalanceCheckAddress;
//             
//             //Act
//             var balance = await client.GetAccountBalanceAsync(address);
//             
//             //Assert
//             Assert.AreEqual(0.2, balance);
//         }
//
//         [Test]
//         public async Task ShouldDeploySimpleStorageContract()
//         {
//             //Arrange
//             var secretValue = 42;
//             var adminProvider = new EthereumAdminAccountConfigurationProvider(_config);
//             var client = new EthereumClient(adminProvider, _config, null);
//             var serviceFactory = new EthereumServiceFactory(adminProvider, _config, null);
//             
//             //Act
//             var address = await client.DeployContractAsync<SimpleStorageDeployment>();
//             
//             //Assert
//             Assert.IsNotEmpty(address);
//             var contract = await serviceFactory.GetServiceAsync<SimpleStorageService>(address);
//             Assert.NotNull(contract);
//             var receiptForSetFunctionCall = await contract.SetRequestAndWaitForReceiptAsync(new SetFunction() { X = secretValue});
//             Assert.NotNull(receiptForSetFunctionCall);
//             var intValueFromGetFunctionCall = await contract.GetQueryAsync();
//             Assert.AreEqual(secretValue, (int)intValueFromGetFunctionCall);
//         }
//
//         [Test]
//         public async Task ShouldEmitEventWhenContractDeployed()
//         {
//             //Arrange
//             var secretValue = 42;
//             var adminProvider = new EthereumAdminAccountConfigurationProvider(_config);
//             var client = new EthereumClient(adminProvider, _config, null);
//             var serviceFactory = new EthereumServiceFactory(adminProvider, _config, null);
//             
//             //Act
//             var address = await client.DeployContractAsync<SimpleStorageDeployment>();
//             
//             //Assert
//             Assert.IsNotEmpty(address);
//             var contract = await serviceFactory.GetServiceAsync<SimpleStorageService>(address);
//             Assert.NotNull(contract);
//             var receiptForSetFunctionCall = await contract.SetRequestAndWaitForReceiptAsync(new SetFunction() { X = secretValue});
//             Assert.NotNull(receiptForSetFunctionCall);
//
//             var accountBalanceBeforeEventHandling = await client.GetAccountBalanceAsync(_testConfiguration.AdminWalletAddress); 
//             
//             var transferEventOutput = receiptForSetFunctionCall.DecodeAllEvents<ValueSetEventDTO>();
//             Assert.AreEqual(1, transferEventOutput.Count);
//             
//             var transferEventHandler = client.GetClient(await serviceFactory.GetAdminAccountAsync()).Eth.GetEvent<ValueSetEventDTO>(address);
//             var filterAllTransferEventsForContract = transferEventHandler.CreateFilterInput();
//             var allTransferEventsForContract = await transferEventHandler.GetAllChangesAsync(filterAllTransferEventsForContract);
//             Assert.AreEqual(1, allTransferEventsForContract.Count);
//             
//             var accountBalanceAfterEventHandled = await client.GetAccountBalanceAsync(_testConfiguration.AdminWalletAddress); 
//             
//             Assert.AreEqual(accountBalanceBeforeEventHandling, accountBalanceAfterEventHandled);
//         }
//         
//         [Test]
//         public async Task ShouldReceiveEventsFromBothContracts()
//         {
//             //Arrange
//             
//             var secretValue = 42;
//             var adminProvider = new EthereumAdminAccountConfigurationProvider(_config);
//             var client = new EthereumClient(adminProvider, _config, null);
//             var serviceFactory = new EthereumServiceFactory(adminProvider, _config, null);
//             var web3 = client.GetClient(await serviceFactory.GetAdminAccountAsync());
//             var transferEventHandlerAnyContract = web3.Eth.GetEvent<ValueSetEventDTO>();
//             var filterAllTransferEventsForAllContracts = transferEventHandlerAnyContract.CreateFilterInput();
//
//             // Act
//             var allTransferEventsBeforeTests = await transferEventHandlerAnyContract.GetAllChangesAsync(filterAllTransferEventsForAllContracts);
//             var address1 = await client.DeployContractAsync<SimpleStorageDeployment>();
//             var contract1 = await serviceFactory.GetServiceAsync<SimpleStorageService>(address1);
//             await contract1.SetRequestAndWaitForReceiptAsync(new SetFunction() { X = secretValue});
//             
//             var address2 = await client.DeployContractAsync<SimpleStorageDeployment>();
//             var contract2 = await serviceFactory.GetServiceAsync<SimpleStorageService>(address2);
//             await contract2.SetRequestAndWaitForReceiptAsync(new SetFunction() { X = secretValue});
//
//             var accountBalanceBeforeEventHandling = await client.GetAccountBalanceAsync(_testConfiguration.AdminWalletAddress);
//             
//             var allTransferEventsForContracts = await transferEventHandlerAnyContract.GetAllChangesAsync(filterAllTransferEventsForAllContracts);
//             
//             var accountBalanceAfterEventHandling = await client.GetAccountBalanceAsync(_testConfiguration.AdminWalletAddress);
//             
//             // Assert
//             Assert.AreEqual(allTransferEventsBeforeTests.Count + 2, allTransferEventsForContracts.Count);
//             Assert.AreEqual(accountBalanceBeforeEventHandling, accountBalanceAfterEventHandling);
//         }
//     }
// }