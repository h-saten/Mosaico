using System;

namespace Mosaico.Integration.Blockchain.Ethereum.Abstractions
{
    public class CreateVaultConfiguration
    {
        public string Recipient { get; set; }
        public string TokenAddress { get; set; }
        public string VaultAddress { get; set; }
        public int Decimals { get; set; }
        public decimal Amount { get; set; }
        public DateTimeOffset AvailableAt { get; set; }
    }
}