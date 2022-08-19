using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Nethereum.ABI.Model;
using Nethereum.Contracts.Services;

namespace Mosaico.Integration.Blockchain.Ethereum.Tools
{
    public class ContractAnalyzer : IContractAnalyzer
    {
        private readonly IEtherScanner _etherScanner;

        public ContractAnalyzer(IEtherScanner etherScanner)
        {
            _etherScanner = etherScanner;
        }

        private async Task<ContractABI> GetContractAbiAsync(string network, string contractAddress)
        {
            var contract = await _etherScanner.TokenContractAsync(network, contractAddress);
            EthApiContractService ethApi = new EthApiContractService(null);
            var contractContent = ethApi.GetContract(contract.Result, contractAddress);
            return contractContent.ContractBuilder.ContractABI;
        }

        public async Task<bool> ContainAllFunctionsAsync(string network, string contractAddress, params string[] functionNames)
        {
            var abi = await GetContractAbiAsync(network, contractAddress);
            var functionsAmountToCheck = functionNames.Length;
            var existingFunctionsCounter = 0;
            foreach (var functionName in functionNames)
            {
                var functionExist = abi.Functions.Any(x => x.Name == functionName);
                if (functionExist is true) existingFunctionsCounter++;
            }

            return functionsAmountToCheck == existingFunctionsCounter;
        }

        public async Task<Dictionary<string, bool>> FunctionsExistsAsync(string network, string contractAddress, string[] functionNames)
        {
            var abi = await GetContractAbiAsync(network, contractAddress);
            var result = new Dictionary<string, bool>();
            foreach (var functionName in functionNames)
            {
                var functionExist = abi.Functions.Any(x => x.Name == functionName);
                result.Add(functionName, functionExist);
            }

            return result;
        }
    }
}