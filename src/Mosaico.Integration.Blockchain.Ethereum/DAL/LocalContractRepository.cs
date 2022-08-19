using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mosaico.Blockchain.Base.DAL;
using Mosaico.Blockchain.Base.DAL.Models;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.Integration.Blockchain.Ethereum.DefaultCrowdsalev1.ContractDefinition;
using Nethereum.Contracts;

namespace Mosaico.Integration.Blockchain.Ethereum.DAL
{
    public class LocalContractRepository : ICrowdSaleRepository
    {
        private readonly IEthereumClientFactory _ethereumClient;

        public LocalContractRepository(IEthereumClientFactory ethereumClient)
        {
            _ethereumClient = ethereumClient;
        }

        public async Task<List<PurchaseConfirmation>> PurchaseConfirmationsAsync(string contractAddress, string chain, DateTimeOffset? fromDate = null)
        {
            var client = _ethereumClient.GetClient(chain);
            var adminAccount = await client.GetAdminAccountAsync();
            var ethApiContractService = client.GetClient(adminAccount).Eth;
            var transferEventHandler = ethApiContractService.GetEvent<TokensPurchasedEventDTO>(contractAddress);
            var filterAllTransferEventsForContract = transferEventHandler.CreateFilterInput();

            var allTransferEventsForContract = await transferEventHandler.GetAllChangesAsync(filterAllTransferEventsForContract);
            var response = new List<PurchaseConfirmation>();
            foreach (var @event in allTransferEventsForContract)
            {
                response.Add(new PurchaseConfirmation
                {
                    Beneficiary = @event.Event.Beneficiary,
                    Payer = @event.Event.Purchaser,
                    PayedAmount = @event.Event.Value,
                    ReceivedTokensAmount = @event.Event.Amount,
                    Date = DateTimeOffset.UtcNow,
                });
            }
            return response;
        }
    }
}