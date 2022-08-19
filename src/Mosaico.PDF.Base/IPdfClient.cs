using System.Threading.Tasks;

namespace Mosaico.PDF.Base
{
    public interface IPdfClient
    {
        Task<byte[]> GenerateAsync(string template, object configuration);
    }
}