using System;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.Integration.Blockchain.Ethereum.Configuration;
using Mosaico.Integration.Blockchain.Ethereum.Exceptions;
using Mosaico.Integration.Blockchain.Ethereum.Models;
using RestSharp;

namespace Mosaico.Integration.Blockchain.Ethereum
{
    public class EtherScanner : IEtherScanner
    {
        private readonly IEthereumClientFactory _ethereumClientFactory;

        public EtherScanner(IEthereumClientFactory ethereumClientFactory)
        {
            _ethereumClientFactory = ethereumClientFactory;
        }

        public async Task<ScanTransactionResponse> GetTransactionsAsync(string network, string address, int skip = 0, int take = 10, string startBlock = "0", string endBlock = "99999999")
        {
            if (skip < 0) skip = 0;
            if (take <= 0) take = 10;
            var pageNumber = (int) Math.Floor((double) skip / take) + 1;

            EthereumNetworkConfiguration config;
            try
            {
                config = _ethereumClientFactory.GetConfiguration(network);
                if (config == null)
                {
                    return new ScanTransactionResponse();
                }
            }
            catch (InvalidNetworkException ex)
            {
                return new ScanTransactionResponse();
            }
            
            var client = GetScanClient(config.EtherscanApiUrl, config.EtherscanApiToken);
           
            var query = new RestRequest(Method.GET);
            query.AddQueryParameter("module", "account");
            query.AddQueryParameter("action", "txlist");
            query.AddQueryParameter("address", address);
            query.AddQueryParameter("page", pageNumber.ToString());
            query.AddQueryParameter("offset", take.ToString());
            query.AddQueryParameter("sort", "asc");
            if (!string.IsNullOrWhiteSpace(startBlock) && BigInteger.TryParse(startBlock, out var blockNumber))
            {
                query.AddQueryParameter("startblock", startBlock);
            }
            
            var rawResponse = await client.ExecuteGetAsync<ScanTransactionResponse>(query);
            if (rawResponse.IsSuccessful)
            {
                return rawResponse.Data;
            }

            throw new EtherScanException(rawResponse.Content);
        }
        
        public async Task<ScanTransactionResponse> GetERC20TransactionsAsync(string network, string address, string contractAddress, int skip = 0, int take = 10, string startBlock = "0", string endBlock = "99999999")
        {
            if (skip < 0) skip = 0;
            if (take <= 0) take = 10;
            var pageNumber = (int) Math.Floor((double) skip / take) + 1;

            EthereumNetworkConfiguration config;
            try
            {
                config = _ethereumClientFactory.GetConfiguration(network);
                if (config == null)
                {
                    return new ScanTransactionResponse();
                }
            }
            catch (InvalidNetworkException ex)
            {
                return new ScanTransactionResponse();
            }
            
            var client = GetScanClient(config.EtherscanApiUrl, config.EtherscanApiToken);
           
            var query = new RestRequest(Method.GET);
            query.AddQueryParameter("module", "account");
            query.AddQueryParameter("action", "tokentx");
            query.AddQueryParameter("address", address);
            query.AddQueryParameter("contractaddress", contractAddress);
            query.AddQueryParameter("sort", "asc");
            query.AddQueryParameter("page", pageNumber.ToString());
            query.AddQueryParameter("offset", take.ToString());
            if (!string.IsNullOrWhiteSpace(startBlock) && BigInteger.TryParse(startBlock, out var blockNumber))
            {
                query.AddQueryParameter("startblock", startBlock);
            }
            
            var rawResponse = await client.ExecuteGetAsync<ScanTransactionResponse>(query);
            if (rawResponse.IsSuccessful)
            {
                return rawResponse.Data;
            }

            throw new EtherScanException(rawResponse.Content);
        }
        
        public async Task<TokenDetails> TokenDetailsAsync(string network, string contractAddress)
        {
            EthereumNetworkConfiguration config;
            try
            {
                config = _ethereumClientFactory.GetConfiguration(network);
                if (config == null)
                {
                    return null;
                }
            }
            catch (InvalidNetworkException)
            {
                return null;
            }
            
            var client = GetScanClient(config.EtherscanApiUrl, config.EtherscanApiToken);
           
            var query = new RestRequest(Method.GET);
            query.AddQueryParameter("module", "token");
            query.AddQueryParameter("action", "tokeninfo");
            query.AddQueryParameter("contractaddress", contractAddress);
            
            var rawResponse = await client.ExecuteGetAsync<TokenInfoResponse>(query);
            if (rawResponse.IsSuccessful)
            {
                
                // TODO return raw result and process value somewhere else
                return rawResponse.Data.Result.First();
            }

            throw new EtherScanException(rawResponse.Content);
        }
        
        public async Task<ContractSourceCodeResponse> TokenContractAsync(string network, string contractAddress)
        {
            EthereumNetworkConfiguration config;
            try
            {
                config = _ethereumClientFactory.GetConfiguration(network);
                if (config == null)
                {
                    return new ContractSourceCodeResponse();
                }
            }
            catch (InvalidNetworkException)
            {
                return new ContractSourceCodeResponse();
            }
            
            var client = GetScanClient(config.EtherscanApiUrl, config.EtherscanApiToken);
           
            var query = new RestRequest(Method.GET);
            query.AddQueryParameter("module", "contract");
            query.AddQueryParameter("action", "getabi");
            query.AddQueryParameter("address", contractAddress);
            
            var rawResponse = await client.ExecuteGetAsync<ContractSourceCodeResponse>(query);
            if (rawResponse.IsSuccessful)
            {
                var cleanedSourceCode = rawResponse.Data.Result.Replace(@"\", string.Empty);
                rawResponse.Data.Result = cleanedSourceCode;
                return rawResponse.Data;
            }

            throw new EtherScanException(rawResponse.Content);
        }

        private RestClient GetScanClient(string url, string apiToken)
        {
            var client = new RestClient(url);
            client.AddDefaultQueryParameter("apikey", apiToken);
            return client;
        }
    }
}