using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mosaico.Export.Base
{
    public interface IExporter<TEntity> where TEntity : class
    {
        Task<byte[]> ExportAsync(List<TEntity> entities);
    }
}