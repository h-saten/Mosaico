using System;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.Wallet.Entities
{
    //TODO: to translatable entity
    public class TokenType : EntityBase
    {
        public TokenType()
        {
            Id = Guid.NewGuid();
        }

        public TokenType(string key, string title)
        {
            Id = Guid.NewGuid();
            Key = key;
            Title = title;
        }

        public string Key { get; set; }
        public string Title { get; set; }
    }
}