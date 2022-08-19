using System.Collections.Generic;

namespace Mosaico.Application.ProjectManagement.Queries.GetTokenomics
{
    public class GetTokenomicsQueryResponse
    {
        public List<string> Labels { get; set; } = new List<string>();
        public List<decimal> Series { get; set; } = new List<decimal>();
        public List<decimal> Percentage { get; set; } = new List<decimal>();
    }
}