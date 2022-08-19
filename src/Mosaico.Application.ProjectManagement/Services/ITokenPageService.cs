using System.Threading.Tasks;
using Mosaico.Domain.ProjectManagement.Entities;

namespace Mosaico.Application.ProjectManagement.Services
{
    public interface ITokenPageService
    {
        Task CreateTokenPageAsync(Project project);
    }
}