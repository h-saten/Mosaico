using System.Collections.Generic;

namespace Mosaico.Application.ProjectManagement.Queries.ProjectScore
{
    public class ProjectScoreQueryResponse
    {
        public double Score { get; set; }
        public List<string> Errors { get; set; }
    }
}