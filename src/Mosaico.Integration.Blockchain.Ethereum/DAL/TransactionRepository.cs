using System.Threading.Tasks;
using Mosaico.Blockchain.Base.DAL;
using Mosaico.Blockchain.Base.DAL.Models;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;

namespace Mosaico.Integration.Blockchain.Ethereum.DAL
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IEthereumClientFactory _ethereumClient;

        public TransactionRepository(IEthereumClientFactory ethereumClient)
        {
            _ethereumClient = ethereumClient;
        }

        public async Task<TransactionReceipt> TransactionByHashAsync(string chain, string transactionHash)
        {
            var client = _ethereumClient.GetClient(chain);
            var adminAccount = await client.GetAdminAccountAsync();
            var ethApiContractService = client.GetClient(adminAccount).Eth;
            var receipt = await ethApiContractService.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash);
            return new TransactionReceipt
            {
                BlockHash = receipt.BlockHash,
                BlockNumber = (ulong) receipt.BlockNumber.Value,
                TransactionHash = receipt.TransactionHash
            };
        }
    }
}