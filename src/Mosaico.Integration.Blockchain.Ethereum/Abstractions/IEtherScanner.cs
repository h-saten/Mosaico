using System.Threading.Tasks;
using Mosaico.Integration.Blockchain.Ethereum.Models;

namespace Mosaico.Integration.Blockchain.Ethereum.Abstractions
{
    public interface IEtherScanner
    {
        Task<ScanTransactionResponse> GetERC20TransactionsAsync(string network, string address, string contractAddress,
            int skip = 0, int take = 10, string startBlock = "0", string endBlock = "99999999");
        Task<ScanTransactionResponse> GetTransactionsAsync(string network, string address, int skip = 0, int take = 10, string startBlock = "0", string endBlock = "99999999");
        Task<TokenDetails> TokenDetailsAsync(string network, string contractAddress);
        Task<ContractSourceCodeResponse> TokenContractAsync(string network, string contractAddress);
    }
}