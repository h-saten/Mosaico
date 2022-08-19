using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Moq;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.Integration.Blockchain.Ethereum.Configuration;
using Mosaico.Integration.Blockchain.Ethereum.DAL;
using Mosaico.Integration.Blockchain.Ethereum.DefaultCrowdsalev1;
using Mosaico.Integration.Blockchain.Ethereum.DefaultCrowdsalev1.ContractDefinition;
using Mosaico.Integration.Blockchain.Ethereum.MosaicoERC20v1;
using Mosaico.Integration.Blockchain.Ethereum.Tests.Configuration;
using Mosaico.Integration.Blockchain.Ethereum.Tests.Helpers;
using Mosaico.Integration.Blockchain.Moralis.ApiClients;
using Mosaico.Tests.Base;
using NUnit.Framework;

namespace Mosaico.Integration.Blockchain.Ethereum.Tests.Crowdsale
{
    public class Crowdsalev1Tests : TestBase
    {
        private BlockchainConfiguration _config;
        private EthereumTestConfiguration _testConfig;
        
        [SetUp]
        public void Setup()
        {
            _config = GetSettings<BlockchainConfiguration>(BlockchainConfiguration.SectionName);
            _testConfig = GetSettings<EthereumTestConfiguration>(EthereumTestConfiguration.SectionName);
        }

        internal class StageParams
        {
            public string Name { get; set; }
            public BigInteger Cap { get; set; }
            public BigInteger NativeCurrencyRate { get; set; }
            public BigInteger StableCoinRate { get; set; }
            public bool IsPrivate { get; set; }
            public BigInteger MinIndividualCap { get; set; }
            public BigInteger MaxIndividualCap { get; set; }
            public List<string> Whitelisted { get; set; }
        }

        internal class CrowdSaleResult
        {
            public EthereumClient EthClient { get; set; }
            public DefaultCrowdsalev1Service CrowdSaleService { get; set; } 
            public string CrowdSaleAddress { get; set; } 
            public MosaicoERC20v1Service CrowdSaleToken { get; set; } 
            public string CrowdSaleTokenAddress { get; set; } 
            public StageParams CurrentStage { get; set; }
            public MosaicoERC20v1Service PaymentToken { get; set; }
            public string PaymentTokenAddress { get; set; }
        }

        private async Task<CrowdSaleResult> DeployCrowdSaleContractWithFirstStage()
        {
            //Arrange
            var config = _config.Networks.FirstOrDefault();
            var adminProvider = new EthereumAdminAccountConfigurationProvider
            {
                Configuration = config
            };
            var client = new EthereumClient(null)
            {
                AdminAccountProvider = adminProvider,
                Configuration = config
            };
            var companyWallet = client.CreateWallet();
            var adminWallet = await client.GetAdminAccountAsync();
            var tether = await client.DeployERC20("Tether", "USDT", 10000000, ownerAddress: adminWallet.Address);

            var softCap = 300;
            var hardCap = 1000;
            var supportedStableCoins = new List<string> { tether };
            var stage = new StageParams
            {
                Cap = 1000,
                Name = "Sale",
                Whitelisted = new List<string>(),
                IsPrivate = false,
                NativeCurrencyRate = 20,
                StableCoinRate = 2,
                MinIndividualCap = 10,
                MaxIndividualCap = 200
            };
            var stages = new List<StageParams> { stage };
            
            var adminAccountDetails = await adminProvider.GetAdminAccountDetailsAsync();
            var adminAccount = await client.GetAccountAsync(adminAccountDetails.PrivateKey);

            var serviceFactory = new EthereumServiceFactory(null)
            {
                Configuration = config,
                AdminAccountProvider = adminProvider
            };
            
            //Act
            var tokenAddress = await client.DeployERC20(ownerAddress: adminAccount.Address);
            
            var settings = DefaultCrowdsalev1Settings.GetSettings(
                companyWallet.Address, 
                tokenAddress, 
                30, 
                supportedStableCoins,
                stages.Count);
            
            var crowdSaleAddress = await client.DeployContractAsync<DefaultCrowdsalev1Deployment>(null, settings);
            var crowdSaleContract = await serviceFactory.GetServiceAsync<DefaultCrowdsalev1Service>(crowdSaleAddress, string.Empty);
            var stageIndex = 0;
            var stageSettings = DefaultCrowdsalev1Settings.GetStartNextStageFunction(
                stages[stageIndex].Name, 
                stages[stageIndex].Cap, 
                stages[stageIndex].NativeCurrencyRate,
                stages[stageIndex].StableCoinRate,
                stages[stageIndex].IsPrivate,
                stages[stageIndex].MinIndividualCap,
                stages[stageIndex].MaxIndividualCap,
                stages[stageIndex].Whitelisted);
            
            //Assert
            Assert.IsNotEmpty(crowdSaleAddress);
            await crowdSaleContract.StartNextStageRequestAndWaitForReceiptAsync(stageSettings);
            
            var tetherContract = await serviceFactory.GetServiceAsync<MosaicoERC20v1Service>(tether, string.Empty);
            var crowdSaleTokenContract = await serviceFactory.GetServiceAsync<MosaicoERC20v1Service>(tokenAddress, string.Empty);

            return new CrowdSaleResult
            {
                EthClient = client,
                CrowdSaleService = crowdSaleContract,
                CrowdSaleAddress = crowdSaleAddress,
                CurrentStage = stage,
                PaymentToken = tetherContract,
                PaymentTokenAddress = tether,
                CrowdSaleToken = crowdSaleTokenContract,
                CrowdSaleTokenAddress = tokenAddress,
            };
        }
        
        [Test]
        public async Task ShouldDeployCrowdSaleContract()
        {
            //Arrange
            var crowdSaleResult = await DeployCrowdSaleContractWithFirstStage();
            Assert.NotNull(crowdSaleResult.CrowdSaleAddress);
            
            var rateCallFunctionResult = await crowdSaleResult.CrowdSaleService.GetRateQueryAsync();
            Assert.NotNull(rateCallFunctionResult);
            Assert.AreEqual(crowdSaleResult.CurrentStage.NativeCurrencyRate, rateCallFunctionResult);
        }

        [Test]
        public async Task InitialWalletBalanceShouldBeZero()
        {
            var crowdSaleResult = await DeployCrowdSaleContractWithFirstStage();
            var beneficiaryWallet = crowdSaleResult.EthClient.CreateWallet();

            var beforeExchangeBalance = await crowdSaleResult.CrowdSaleService.BalanceOfQueryAsync(beneficiaryWallet.Address);
            Assert.AreEqual(new BigInteger(0), beforeExchangeBalance);
        }

        [Test]
        public async Task ShouldReturnValidCrowdSaleParams()
        {
            var crowdSaleResult = await DeployCrowdSaleContractWithFirstStage();

            var token = await crowdSaleResult.CrowdSaleService.GetTokenQueryAsync();
            var nativeCurrencyRate = await crowdSaleResult.CrowdSaleService.GetRateQueryAsync();
            Assert.IsTrue(string.Equals(crowdSaleResult.CrowdSaleTokenAddress, token, StringComparison.InvariantCultureIgnoreCase));
            Assert.AreEqual(crowdSaleResult.CurrentStage.NativeCurrencyRate, nativeCurrencyRate);
        }
        
        [Test]
        public async Task ShouldAssignUserValidTokensAmountBasedOnNativeCurrencyRate()
        {
            var crowdSaleResult = await DeployCrowdSaleContractWithFirstStage();
            var beneficiaryWallet = crowdSaleResult.EthClient.CreateWallet();

            var exchangeFunction = new BuyTokensFunction
            {
                Beneficiary = beneficiaryWallet.Address,
                AmountToSend = new BigInteger(200),
                Gas = new BigInteger(300000),
                GasPrice = 1
            };
            await crowdSaleResult.CrowdSaleService.BuyTokensRequestAndWaitForReceiptAsync(exchangeFunction);

            var beneficiaryResult =
            await crowdSaleResult.CrowdSaleService.BalanceOfQueryAsync(beneficiaryWallet.Address);
            
            Assert.IsTrue(new BigInteger(10).Equals(beneficiaryResult));
        }

        [Test]
        public async Task ShouldTransferTokensToCrowdSaleAddress()
        {
            var crowdSaleResult = await DeployCrowdSaleContractWithFirstStage();
            var beneficiaryWallet = crowdSaleResult.EthClient.CreateWallet();
            await crowdSaleResult.PaymentToken.ApproveRequestAndWaitForReceiptAsync(crowdSaleResult.CrowdSaleAddress, 1000);

            var exchangeFunction = new ExchangeTokensFunction
            {
                PaymentToken = crowdSaleResult.PaymentTokenAddress,
                Amount = new BigInteger(100),
                Gas = new BigInteger(300000),
            };
            await crowdSaleResult.CrowdSaleService.ExchangeTokensRequestAndWaitForReceiptAsync(exchangeFunction);

            var beneficiaryResult =
                await crowdSaleResult.CrowdSaleService.BalanceOfQueryAsync(beneficiaryWallet.Address);
            
            Assert.IsTrue(new BigInteger(50).Equals(beneficiaryResult));
        }

        [Test]
        public async Task ShouldTransferTokensToCrowdSaleAddressAndFindConfirmationEvent()
        {
            var crowdSaleResult = await DeployCrowdSaleContractWithFirstStage();
            var beneficiaryWallet = crowdSaleResult.EthClient.CreateWallet();
            await crowdSaleResult.PaymentToken.ApproveRequestAndWaitForReceiptAsync(crowdSaleResult.CrowdSaleAddress, 1000);

            var exchangeFunction = new ExchangeTokensFunction
            {
                PaymentToken = crowdSaleResult.PaymentTokenAddress,
                Amount = new BigInteger(100),
                Gas = new BigInteger(300000),
            };
            await crowdSaleResult.CrowdSaleService.ExchangeTokensRequestAndWaitForReceiptAsync(exchangeFunction);

            var beneficiaryResult =
                await crowdSaleResult.CrowdSaleService.BalanceOfQueryAsync(beneficiaryWallet.Address);
            
            Assert.IsTrue(new BigInteger(50).Equals(beneficiaryResult));

            var ethClientFactory = new Mock<IEthereumClientFactory>();
            ethClientFactory.Setup(x => x.GetClient(It.IsAny<string>())).Returns(crowdSaleResult.EthClient);
            var contractRepository = new LocalContractRepository(ethClientFactory.Object);
            var confirmations = await contractRepository.PurchaseConfirmationsAsync(crowdSaleResult.CrowdSaleAddress, "Ethereum");
            var transactionConfirmation =
                confirmations.FirstOrDefault(x =>
                    x.Beneficiary == beneficiaryWallet.Address
                    && x.PayedAmount == exchangeFunction.Amount);
            Assert.NotNull(transactionConfirmation);
        }
    }
}
