using System.Threading.Tasks;
using RestSharp;

namespace Mosaico.Base.Abstractions
{
    public interface IRestClientInterceptor
    {
        Task<IRestClient> InterceptAsync(IRestClient client);
    }
}