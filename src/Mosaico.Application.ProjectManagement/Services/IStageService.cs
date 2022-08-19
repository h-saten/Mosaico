using System.Threading.Tasks;
using Mosaico.Domain.ProjectManagement.Entities;

namespace Mosaico.Application.ProjectManagement.Services
{
    public interface IStageService
    {
        Task StartStageAsync(Stage stage);
        Task SetStagePending(Stage stage);
        Task SetStageClosed(Stage stage);
    }
}