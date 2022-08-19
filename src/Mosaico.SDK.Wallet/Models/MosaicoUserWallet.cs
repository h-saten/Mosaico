using System;
using System.Collections.Generic;

namespace Mosaico.SDK.Wallet.Models
{
    public class MosaicoUserWallet
    {
        public Guid Id { get; set; }
        public List<MosaicoToken> Tokens { get; set; }
        public string AccountAddress { get; set; }
        public string Network { get; set; }
        public string UserId { get; set; }
    }
}