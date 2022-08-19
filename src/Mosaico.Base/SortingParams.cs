using Mosaico.Base.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mosaico.Base
{
    public class SortingParams
    {
        public string ColumnName { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonIgnore] public SortOrders SortOrder { get; set; } = SortOrders.Asc;
    }
}