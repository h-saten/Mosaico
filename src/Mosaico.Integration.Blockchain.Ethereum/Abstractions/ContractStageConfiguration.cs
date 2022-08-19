using System.Collections.Generic;
using System.Numerics;

namespace Mosaico.Integration.Blockchain.Ethereum.Abstractions
{
    public class ContractStageConfiguration
    {
        public string Name { get; set; }
        public bool IsPrivate { get; set; }
        public decimal Cap { get; set; }
        public decimal MinIndividualCap { get; set; }
        public decimal MaxIndividualCap { get; set; }
        public List<string> Whitelist { get; set; } = new List<string>();
        public virtual decimal Rate { get; set; }
        public virtual decimal StableCoinRate { get; set; }
        public string PrivateKey { get; set; }
    }
}