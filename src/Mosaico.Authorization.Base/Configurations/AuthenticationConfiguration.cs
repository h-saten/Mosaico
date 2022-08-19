using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mosaico.Authorization.Base.Configurations
{
    public class AuthenticationConfiguration
    {
        public const string SectionName = "Service";

        [JsonProperty("RedirectUris")]
        public List<string> RedirectUris { get; set; } = new() { };
    }
}