using Mosaico.Blockchain.Base.DAL.Events;

namespace Mosaico.Blockchain.Base.DAL.Models
{
    public class ContractEvent<TEvent> where TEvent : class, IContractEvent
    {
        public string TransactionHash { get; set; }
        public string BlockNumber { get; set; }
        public string BlockTimestamp { get; set; }
        public string BlockHash { get; set; }
        public TEvent Payload { get; set; }
    }
}