using System.Reflection;
using System.Threading.Tasks;

namespace Mosaico.Base.Abstractions
{
    public interface IResourceManager
    {
        Task<string> GetResourceNameAsync(string category, string name, string culture = Constants.Languages.English, Assembly assembly = null);
        Task<byte[]> GetResourceAsync(string category, string name, string culture = Constants.Languages.English, Assembly assembly = null);
        Task<string> GetTextResourceAsync(string category, string name, string culture = Constants.Languages.English, Assembly assembly = null);
    }
}