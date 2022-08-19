using System.Threading.Tasks;
using KangaExchange.SDK.Models;
using RestSharp;

namespace KangaExchange.SDK.Abstractions
{
    public interface IKangaIssuerApiClient
    {
        Task<IRestResponse<ReportResponseDto>> ReportAsync(string tokenTicker);
    }
}