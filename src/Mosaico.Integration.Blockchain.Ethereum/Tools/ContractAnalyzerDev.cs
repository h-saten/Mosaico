using System.Collections.Generic;
using System.Threading.Tasks;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;

namespace Mosaico.Integration.Blockchain.Ethereum.Tools
{
    public class ContractAnalyzerDev : IContractAnalyzer
    {
        public async Task<bool> ContainAllFunctionsAsync(string network, string contractAddress, params string[] functionNames)
        {
            return await Task.FromResult(true);
        }

        public async Task<Dictionary<string, bool>> FunctionsExistsAsync(string network, string contractAddress, string[] functionNames)
        {
            var response = new Dictionary<string, bool>();
            foreach (var functionName in functionNames)
            {
                response.Add(functionName, true);
            }
            return await Task.FromResult(response);
        }
    }
}