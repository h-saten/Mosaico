using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Mosaico.Base.Abstractions;

namespace Mosaico.Base
{
    public class AutofacUrlResolver : IUrlResolver
    {
        private readonly IIndex<string, string> _urls;

        public AutofacUrlResolver(IIndex<string, string> urls = null)
        {
            _urls = urls;
        }

        public Task<string> GetUrlAsync(string key)
        {
            var url = string.Empty;
            if (_urls != null && !string.IsNullOrWhiteSpace(key))
            {
                _urls.TryGetValue(key, out url);
            }
            return Task.FromResult(url);
        }
    }
}