using Mosaico.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaico.Domain.DocumentManagement.Entities
{
    public abstract class DocumentBase : EntityBase
    {
        public string Title { get; set; }
        public virtual ICollection<DocumentContent> Contents { get; set; } = new List<DocumentContent>();
        public abstract void SetRelatedEntityId(Guid id);
    }
}
