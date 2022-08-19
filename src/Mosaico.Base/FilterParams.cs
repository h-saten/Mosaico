using Mosaico.Base.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mosaico.Base
{
    public class FilterParams
    {
        public string ColumnName { get; set; } = string.Empty;  
        public string FilterValue { get; set; } = string.Empty;  
        [JsonConverter(typeof(StringEnumConverter))]
        public FilterOptions FilterOption { get; set; } = FilterOptions.Contains; 
    }
}