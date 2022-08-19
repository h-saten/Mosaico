using System.Collections.Generic;
using System.Numerics;
using Mosaico.Integration.Blockchain.Ethereum.MosaicoERC20v1.ContractDefinition;

namespace Mosaico.Integration.Blockchain.Ethereum.MosaicoERC20v1
{
    public static class MosaicoERC20v1Extensions
    {
        public static Dictionary<string, object> GetSettings(string name, string symbol, BigInteger initialSupply,
            bool isMintable = false, bool isBurnable = false,
            string walletAddress = Constants.DefaultWalletAddress, bool isGovernance = false)
        {
            return new Dictionary<string, object>
            {
                {"settings", new ERC20Settings
                {
                    Name = name,
                    Symbol = symbol,
                    InitialSupply = initialSupply,
                    IsBurnable = isBurnable,
                    IsMintable = isMintable,
                    WalletAddress = walletAddress,
                    IsGovernance = isGovernance
                }}
            };
        }
    }
}