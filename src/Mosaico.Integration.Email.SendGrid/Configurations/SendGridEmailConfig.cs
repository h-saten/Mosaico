using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaico.Integration.Email.SendGridEmail.Configurations
{
    public class SendGridEmailConfig
    {
        public const string SectionName = "SendGridEmail";

        [JsonProperty("ApiKey")]
        public string AppKey { get; set; }

        [JsonProperty("FromEmail")]
        public string FromEmail { get; set; }

        [JsonProperty("DisplayName")]
        public string DisplayName { get; set; }
    }
}
