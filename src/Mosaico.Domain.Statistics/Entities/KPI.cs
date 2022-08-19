using Mosaico.Domain.Base;

namespace Mosaico.Domain.Statistics.Entities
{
    public class KPI : EntityBase
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}