using System.Threading.Tasks;

namespace Mosaico.BackgroundJobs.Base
{
    public interface IBackgroundJob
    {
        Task ExecuteAsync(object parameters = null);
    }
}