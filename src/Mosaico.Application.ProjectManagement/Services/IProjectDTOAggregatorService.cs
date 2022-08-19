using System.Threading.Tasks;
using Mosaico.Application.ProjectManagement.DTOs;
using Mosaico.Domain.ProjectManagement.Entities;

namespace Mosaico.Application.ProjectManagement.Services
{
    public interface IProjectDTOAggregatorService
    {
        Task<ProjectDTO> FillInDTOAsync(Project project, ProjectDTO input = null);
        Task<ProjectDetailDTO> FillInDetailDTOAsync(Project project);
    }
}