using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Mosaico.Blockchain.Base.DAL;
using Mosaico.Blockchain.Base.DAL.Models;
using Mosaico.Integration.Blockchain.Moralis.ApiClients.Models;
using Mosaico.Integration.Blockchain.Moralis.Configuration;
using Mosaico.Integration.Blockchain.Moralis.Mappers;
using Nethereum.Web3;
using RestSharp;

namespace Mosaico.Integration.Blockchain.Moralis.ApiClients
{
    public class MoralisTokenApiClient : ITokenRepository
    {
        private readonly RestClient _restClient;
        private readonly MoralisConfiguration _moralisConfiguration;

        public MoralisTokenApiClient(MoralisConfiguration moralisConfiguration)
        {
            _moralisConfiguration = moralisConfiguration;
            _restClient = new RestClient(moralisConfiguration.BasePath);
        }
        
        public async Task<List<ERC20Transfer>> Erc20TransfersAsync(string contractAddress, string chain, DateTimeOffset? fromDate = null)
        {
            var moralisChain = ChainMapper.Map(chain);
            var request = new RestRequest($"erc20/{contractAddress}/transfers?chain={moralisChain}", DataFormat.Json);
            request.AddHeader("X-API-Key", _moralisConfiguration.ApiKey);
            
            var apiResponse = await _restClient.GetAsync<Erc20TransfersResponse>(request);

            return MapMoralisSchemaToResponseType(apiResponse.Result);
        }

        public async Task<List<ERC20Transfer>> Erc20TransfersAsync(string contractAddress, string chain, ulong? fromBlock = null, ulong? toBlock = null)
        {
            var moralisChain = ChainMapper.Map(chain);

            var requestPath = $"erc20/{contractAddress}/transfers?chain={moralisChain}";
            if (fromBlock is not null) requestPath += $"&from_block={fromBlock}";
            if (toBlock is not null) requestPath += $"&to_block={toBlock}";
            
            var request = new RestRequest(requestPath, DataFormat.Json);
            request.AddHeader("X-API-Key", _moralisConfiguration.ApiKey);
            
            var apiResponse = await _restClient.GetAsync<Erc20TransfersResponse>(request);

            return MapMoralisSchemaToResponseType(apiResponse.Result);
        }

        private List<ERC20Transfer> MapMoralisSchemaToResponseType(List<MoralisERC20Transfer> apiResponse)
        {
            var response = new List<ERC20Transfer>();
            foreach (var moralisErc20Transfer in apiResponse)
            {
                response.Add(new ERC20Transfer
                {
                    Address = moralisErc20Transfer.Address,
                    Value = Web3.Convert.FromWei(int.Parse(moralisErc20Transfer.Value)),
                    BlockHash = moralisErc20Transfer.BlockHash,
                    BlockNumber = BigInteger.Parse(moralisErc20Transfer.BlockNumber),
                    Date = DateTimeOffset.FromUnixTimeSeconds(long.Parse(moralisErc20Transfer.BlockTimestamp)),
                    FromAddress = moralisErc20Transfer.FromAddress,
                    ToAddress = moralisErc20Transfer.ToAddress,
                    TransactionHash = moralisErc20Transfer.TransactionHash,
                });
            }

            return response;
        }
    }
}