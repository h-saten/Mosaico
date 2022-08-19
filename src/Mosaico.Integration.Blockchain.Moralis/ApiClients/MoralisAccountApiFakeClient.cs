using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Threading.Tasks;
using Mosaico.Blockchain.Base.DAL;
using Mosaico.Blockchain.Base.DAL.Models;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.Integration.Blockchain.Ethereum.GenericMosaicoUpgradable;
using Mosaico.Integration.Blockchain.Ethereum.MosaicoERC20v1;

namespace Mosaico.Integration.Blockchain.Moralis.ApiClients
{
    public class MoralisAccountApiFakeClient : IAccountRepository
    {
        private readonly IEthereumClientFactory _ethereumClient;

        public MoralisAccountApiFakeClient(IEthereumClientFactory ethereumClient)
        {
            _ethereumClient = ethereumClient;
        }

        public async Task<NativeBalance> AccountBalanceAsync(string walletAddress, string chain)
        {
            var client = _ethereumClient.GetClient(chain);
            var result = await client.GetAccountBalanceAsync(walletAddress);
            return new NativeBalance
            {
                Balance = result
            };
        }
        
        public async Task<BigInteger> Erc20BalanceAsync(string walletAddress, string tokenAddress, string chain, string tokenType = "ERC20")
        {
            var service = _ethereumClient.GetServiceFactory(chain);
            BigInteger result = default;
            if (tokenType == "ERC20")
            {
                var contract = await service.GetServiceAsync<MosaicoERC20v1Service>(tokenAddress, string.Empty);
                result = await contract.BalanceOfQueryAsync(walletAddress);
            }
            else if (tokenType == "ERC1155")
            {
                var contract = await service.GetServiceAsync<GenericMosaicoUpgradableService>(tokenAddress, string.Empty);
                result = await contract.BalanceOfQueryAsync(walletAddress, BigInteger.One);
            }
            else
            {
                throw new Exception($"Unsupported ERC standard");
            }

            return result;
        }
    }
}