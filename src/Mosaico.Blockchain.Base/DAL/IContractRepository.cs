using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mosaico.Blockchain.Base.DAL.Events;
using Mosaico.Blockchain.Base.DAL.Models;

namespace Mosaico.Blockchain.Base.DAL
{
    public interface IContractRepository
    {
        Task<List<ContractEvent<TEvent>>> Events<TEvent>(string contractAddress, string chain, DateTimeOffset? fromDate = null) where TEvent : class, IContractEvent;
    }
}