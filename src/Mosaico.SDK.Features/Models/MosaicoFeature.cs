using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaico.SDK.Features.Models
{
    public class MosaicoFeature
    {
        public Guid Id { get; set; }
        public string FeatureName { get; set; }
        public Guid? EntityId { get; set; }
        public string Value { get; set; }
        public bool IsGloballyAvailable { get; set; }
    }
}
