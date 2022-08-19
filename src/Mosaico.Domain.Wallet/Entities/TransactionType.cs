using System;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.Wallet.Entities
{
    public class TransactionType : EntityBase
    {
        public TransactionType()
        {
            Id = Guid.NewGuid();
        }

        public TransactionType(string key, string title)
        {
            Id = Guid.NewGuid();
            Key = key;
            Title = title;
        }
        
        public string Key { get; set; }
        public string Title { get; set; }
    }
}