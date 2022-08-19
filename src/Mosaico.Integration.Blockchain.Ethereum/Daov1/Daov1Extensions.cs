using System.Collections.Generic;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.Integration.Blockchain.Ethereum.Daov1.ContractDefinition;

namespace Mosaico.Integration.Blockchain.Ethereum.Daov1
{
    public static class Daov1Extensions
    {
        public static Dictionary<string, object> GetSettings(DaoConfiguration config)
        {
            return new Dictionary<string, object>
            {
                {"settings", new DaoSettings
                {
                    Name = config.Name,
                    Owner = config.Owner,
                    Quorum = config.Quorum,
                    InitialVotingDelay = config.InitialVotingDelay,
                    InitialVotingPeriod = config.InitialVotingPeriod,
                    IsVotingEnabled = config.IsVotingEnabled,
                    OnlyOwnerProposals = config.OnlyOwnerProposals
                }}
            };
        }
    }
}