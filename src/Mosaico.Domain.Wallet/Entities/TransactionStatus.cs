using System;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.Wallet.Entities
{
    public class TransactionStatus : EntityBase
    {
        public TransactionStatus()
        {
            Id = Guid.NewGuid();
        }

        public TransactionStatus(string key, string title)
        {
            Id = Guid.NewGuid();
            Key = key;
            Title = title;
        }
        
        public string Key { get; set; }
        public string Title { get; set; }
    }
}