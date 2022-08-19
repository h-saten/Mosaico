using System.Threading.Tasks;
using Mosaico.Base;

namespace Mosaico.Storage.Base
{
    public interface IStorageClient
    {
        Task<StorageObject> GetObjectAsync(string id, string container);
        Task<PaginatedResult<StorageItem>> GetObjectsAsync(string container, int take = 10, int skip = 0);
        Task<StorageItem> CreateAsync(string fileName, byte[] content, string container, bool generateFileName = true, bool overwrite = true);
        Task<string> CreateAsync(StorageObject obj, bool generateFileName = true, bool overwrite = true);
        Task DeleteAsync(string id, string container);
        Task DeleteAsync(StorageObject obj);
        Task<string> GetFileURLAsync(string id, string container);
    }
}