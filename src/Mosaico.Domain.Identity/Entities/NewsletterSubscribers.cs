using System;
using System.Collections.Generic;
using Mosaico.Domain.Base;
using Newtonsoft.Json;

namespace Mosaico.Domain.Identity.Entities
{
    public class NewsletterSubscribers : EntityBase
    {
        public string Email { get; set; }
        [JsonIgnore]
        public bool IsAccepted { get; set; }
    }
}
