using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mosaico.Blockchain.Base.DAL;
using Mosaico.Blockchain.Base.DAL.Models;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.Integration.Blockchain.Ethereum.MosaicoERC20v1.ContractDefinition;
using Nethereum.BlockchainProcessing.BlockStorage.Entities.Mapping;
using Nethereum.Contracts;
using Nethereum.Contracts.Services;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;

namespace Mosaico.Integration.Blockchain.Ethereum.DAL
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IEthereumClientFactory _ethereumClient;
        private readonly IBlockWithTransactionRepository _blockWithTransactionRepository;

        public TokenRepository(IEthereumClientFactory ethereumClient, IBlockWithTransactionRepository blockWithTransactionRepository)
        {
            _ethereumClient = ethereumClient;
            _blockWithTransactionRepository = blockWithTransactionRepository;
        }

        public async Task<List<ERC20Transfer>> Erc20TransfersAsync(string contractAddress, string chain, ulong? fromBlock = null, ulong? toBlock = null)
        {
            var client = _ethereumClient.GetClient(chain);
            var adminAccount = await client.GetAdminAccountAsync();
            var ethApiContractService = client.GetClient(adminAccount).Eth;
            var transferEventHandler = ethApiContractService.GetEvent<TransferEventDTO>(contractAddress);
            var filterAllTransferEventsForContract = transferEventHandler.CreateFilterInput(
                null,
                fromBlock is not null ? new BlockParameter((ulong) fromBlock) : BlockParameter.CreateEarliest(),
                toBlock is not null ? new BlockParameter((ulong) toBlock) : BlockParameter.CreateLatest());
            
            var allTransferEventsForContract = await transferEventHandler.GetAllChangesAsync(filterAllTransferEventsForContract);

            var response = await MapContractEventsToErc20TransactionAsync(ethApiContractService,allTransferEventsForContract);
            
            return response;
        }

        private async Task<List<ERC20Transfer>> MapContractEventsToErc20TransactionAsync(IEthApiContractService ethApiContractService, List<EventLog<TransferEventDTO>> events)
        {
            var response = new List<ERC20Transfer>();
            foreach (var transactionEvent in events)
            {
                var transfer = new ERC20Transfer
                {
                    Address = transactionEvent.Log.Address,
                    Value = Web3.Convert.FromWei(new HexBigInteger(transactionEvent.Event.Value)),
                    TransactionHash = transactionEvent.Log.TransactionHash,
                    BlockHash = transactionEvent.Log.BlockHash,
                    BlockNumber = transactionEvent.Log.BlockNumber.Value,
                    FromAddress = transactionEvent.Event.From,
                    ToAddress = transactionEvent.Event.To
                };
                response.Add(transfer);
            }

            response = await FetchTimestampsAsync(ethApiContractService, response);

            return response;
        }

        private async Task<List<ERC20Transfer>> FetchTimestampsAsync(IEthApiContractService ethApiContractService, List<ERC20Transfer> transfers)
        {
            foreach (var transactionBatch in transfers.GroupBy(t => t.BlockHash))
            {
                var blockHash = transactionBatch.FirstOrDefault()?.BlockHash;
                if (!string.IsNullOrWhiteSpace(blockHash))
                {
                    var transactionBlock = await _blockWithTransactionRepository.GetAsync<BlockWithTransactions>("ethereum", blockHash);
                    if (transactionBlock is null)
                    {
                        transactionBlock = await ethApiContractService.Blocks.GetBlockWithTransactionsByHash.SendRequestAsync(blockHash);
                        if (transactionBlock is not null)
                        { 
                            await _blockWithTransactionRepository.AddAsync<BlockWithTransactions>("ethereum", blockHash, transactionBlock);
                        }
                    }
                    var timestamp = transactionBlock.Timestamp.ToLong();
                    foreach (var transfer in transactionBatch)
                    {
                        transfer.Date = DateTimeOffset.FromUnixTimeSeconds(timestamp);
                    }
                }
            }

            return transfers;
        }
    }
}