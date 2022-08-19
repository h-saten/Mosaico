using System.Threading.Tasks;

namespace Mosaico.Base.Abstractions
{
    public interface IUrlResolver
    {
        Task<string> GetUrlAsync(string key);
    }
}