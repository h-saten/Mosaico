using System;
using System.Threading.Tasks;

namespace Mosaico.Application.ProjectManagement.Abstractions
{
    public interface ICrowdsaleDeploymentJob
    {
        Task DeployCrowdsaleAsync(Guid projectId, string userId);
    }
}