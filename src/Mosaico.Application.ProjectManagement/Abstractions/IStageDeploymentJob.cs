using System;
using System.Threading.Tasks;

namespace Mosaico.Application.ProjectManagement.Abstractions
{
    public interface IStageDeploymentJob
    {
        Task DeployStageAsync(Guid id, string userId);
    }
}