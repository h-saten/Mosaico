using System.Threading.Tasks;
using KangaExchange.SDK.Models;
using RestSharp;

namespace KangaExchange.SDK.Abstractions
{
    public interface IKangaAuthAPIClient
    {
        Task<IRestResponse<CheckResponseDTO>> CheckAsync(string token);
        Task<IRestResponse<ProfileResponseDTO>> ProfileAsync(string userId);
    }
}