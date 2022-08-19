using Mosaico.Integration.Blockchain.Ethereum.Models;

namespace Mosaico.Integration.Blockchain.Ethereum.Abstractions
{
    public static class Daov1Configurations
    {
        public class MintTokenConfiguration
        {
            public string PrivateKey { get; set; }
            public decimal Amount { get; set; }
            public string ContractAddress { get; set; }
            public string DaoAddress { get; set; }
            public int Decimals { get; set; }
        }
        
        public class BurnTokenConfiguration
        {
            public string PrivateKey { get; set; }
            public decimal Amount { get; set; }
            public string ContractAddress { get; set; }
            public string DaoAddress { get; set; }
            public int Decimals { get; set; }
        }

        public class CreateProposalConfiguration
        {
            public string DaoAddress { get; set; }
            public string PrivateKey { get; set; }
            public string Description { get; set; }
            public string ContractAddress { get; set; }
        }

        public class VoteConfiguration
        {
            public string DaoAddress { get; set; }
            public string PrivateKey { get; set; }
            public string ProposalId { get; set; }
            public VoteResult Result { get; set; }
        }

        public class AddERC20Configuration
        {
            public string DaoAddress { get; set; }
            public string PrivateKey { get; set; }
            public string ContractAddress { get; set; }
            public bool IsGovernance { get; set; }
        }

        public class CreateERC20Configuration
        {
            public string DaoAddress { get; set; }
            public string PrivateKey { get; set; }
            public virtual bool IsMintable { get; set; }
            public virtual bool IsBurnable { get; set; }
            public virtual string Name { get; set; }
            public virtual string Symbol { get; set; }
            public virtual decimal InitialSupply { get; set; }
            public virtual bool IsPaused { get; set; }
            public virtual string Url { get; set; }
            public virtual bool IsGovernance { get; set; }
            public int Decimals { get; set; }
            public string WalletAddress { get; set; }
        }
    }
}