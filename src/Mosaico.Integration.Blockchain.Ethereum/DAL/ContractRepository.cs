using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mosaico.Blockchain.Base.DAL;
using Mosaico.Blockchain.Base.DAL.Models;
using Mosaico.Integration.Blockchain.Ethereum.Crowdsale.Events;

namespace Mosaico.Integration.Blockchain.Ethereum.DAL
{
    public class ContractRepository : ICrowdSaleRepository
    {
        private readonly IContractRepository _contractRepository;

        public ContractRepository(IContractRepository contractRepository)
        {
            _contractRepository = contractRepository;
        }

        public async Task<List<PurchaseConfirmation>> PurchaseConfirmationsAsync(string contractAddress, string chain, DateTimeOffset? fromDate = null)
        {
            var events = await _contractRepository.Events<TokensPurchased>(contractAddress, chain, fromDate);
            var response = new List<PurchaseConfirmation>();
            
            foreach (var @event in events)
            {
                response.Add(new PurchaseConfirmation
                {
                    Beneficiary = @event.Payload.Beneficiary,
                    Date = DateTimeOffset.UtcNow,
                    Payer = @event.Payload.Purchaser, // TODO to refactor toward block timestamp
                    PayedAmount = @event.Payload.Value,
                    ReceivedTokensAmount = @event.Payload.Amount
                });
            }

            return response;
        }
    }
}