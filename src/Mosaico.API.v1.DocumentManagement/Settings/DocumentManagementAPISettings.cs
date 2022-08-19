using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaico.API.v1.DocumentManagement.Settings
{
    public class DocumentManagementAPISettings
    {
        public const string SectionName = "DocumentManagementAPI";

        [JsonProperty("MaximumFileSize_Bytes")]
        public long MaximumFileSize_Bytes { get; set; }
        
        [JsonProperty("PermittedExtensions")]
        public IEnumerable<string> PermittedExtensions { get; set; }
    }
}
