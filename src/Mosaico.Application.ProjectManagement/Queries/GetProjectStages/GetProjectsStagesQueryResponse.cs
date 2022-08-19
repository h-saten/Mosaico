using System.Collections.Generic;
using Mosaico.Application.ProjectManagement.DTOs;

namespace Mosaico.Application.ProjectManagement.Queries.GetProjectStages
{
    public class GetProjectsStagesQueryResponse
    {
        public List<StageDTO> Stages { get; set; } = new List<StageDTO>();
    }
}