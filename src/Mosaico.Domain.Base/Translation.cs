
using System;

namespace Mosaico.Domain.Base
{
    public abstract class TranslationBase : EntityBase
    {
        public Guid EntityId { get; set; }
        public string Language { get; set; }
        public string Value { get; set; }
    }
}