using System;
using Mosaico.Core.EntityFramework.Attributes;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.BusinessManagement.Entities
{
    public class SocialMediaLinks : EntityBase
    {
        public Guid CompanyId { get; set; }
        public virtual Company Company { get; set; }
        public string Telegram { get; set; }
        public string Youtube { get; set; }
        public string LinkedIn { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }
        public string Medium { get; set; }

    }
}