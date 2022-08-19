using System;
using System.Collections.Generic;
using Mosaico.Integration.Blockchain.Ethereum.Daov1.ContractDefinition;
using Mosaico.Integration.Blockchain.Ethereum.DefaultCrowdsalev1.ContractDefinition;
using Mosaico.Integration.Blockchain.Ethereum.MosaicoERC20v1.ContractDefinition;
using Mosaico.Integration.Blockchain.Ethereum.StakingUpgradable.ContractDefinition;

namespace Mosaico.Integration.Blockchain.Ethereum
{
    public static class Constants
    {
        public static class BlockTime
        {
            public static double GetBlocksForDay(double blockTime, int numOfDays = 1)
            {
                return (double) numOfDays * 24 * 60 * 60 / blockTime;
            }
            
            public static double GetBlocksForMinutes(double blockTime, int numOfMinutes = 1)
            {
                return (double) numOfMinutes * 60 / blockTime;
            }
        }
        
        public const string DefaultTokenContractVersion = TokenContractVersions.Version1;
        public const string DefaultCrowdsaleContractVersion = CrowdsaleContractVersions.Version1;
        public const string DefaultStakingContractVersion = StakingContractVersions.Version1;
        
        public const string DefaultWalletAddress = "0x0000000000000000000000000000000000000000";

        public static class EthereumAdminAccountProviderTypes
        {
            public const string Configuration = "CONFIGURATION";
        }
        
        public static class DAOContractVersions
        {
            public static Dictionary<string, Type> DeploymentTypes = new Dictionary<string, Type>
            {
                {Version1, typeof(Daov1Deployment)}
            };
            
            public static List<string> All = new List<string> {Version1};
            public const string Version1 = "dao_v1";
        }

        public static class TokenContractVersions
        {
            public static Dictionary<string, Type> DeploymentTypes = new Dictionary<string, Type>
            {
                {Version1, typeof(MosaicoERC20v1Deployment)}
            };
            
            public static List<string> All = new List<string> {Version1};
            public const string Version1 = "erc20_v1";
        }

        public static class CrowdsaleContractVersions
        {
            public static Dictionary<string, Type> DeploymentTypes = new Dictionary<string, Type>
            {
                {Version1, typeof(DefaultCrowdsalev1Deployment)}
            };
            
            public static List<string> All = new List<string> {Version1};
            public const string Version1 = "crowdsale_v1";
        }

        public static class StakingContractVersions
        {
            public static Dictionary<string, Type> DeploymentTypes = new Dictionary<string, Type>
            {
                {Version1, typeof(StakingUpgradableDeployment)}
            };
            
            public static List<string> All = new List<string> {Version1};
            public const string Version1 = "staking_v1";
        }

        public static class ErrorCodes
        {
            public const string AccountNotFound = "ACCOUNT_NOT_FOUND";
            public const string ContractNotFound = "CONTRACT_NOT_FOUND";
            public const string InvalidNetwork = "INVALID_NETWORK";
            public const string TransactionFailed = "ETHEREUM_TRANSACTION_FAILED";
            public const string InvalidContractVersion = "INVALID_CONTRACT_VERSION";
        }
        
        public static class PaymentCurrencies
        {
            public const string USDT = "USDT";
        }
        
        public static class ERC20ContractFunctions
        {
            public const string Burn = "burn";
            public const string Mint = "mint";
        }
    }
}