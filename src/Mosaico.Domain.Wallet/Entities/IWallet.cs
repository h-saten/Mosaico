using Mosaico.Core.EntityFramework.Attributes;

namespace Mosaico.Domain.Wallet.Entities
{
    public interface IWallet
    {
        [Encrypted]
        public string PrivateKey { get; set; }
        public string AccountAddress { get; set; }
        public string Network { get; set; }
        
        [Encrypted]
        public string PublicKey { get; set; }
        public string LastSyncBlockHash { get; set; }
    }
}