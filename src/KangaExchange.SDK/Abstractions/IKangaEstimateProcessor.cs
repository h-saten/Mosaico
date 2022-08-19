using KangaExchange.SDK.Models;

namespace KangaExchange.SDK.Abstractions
{
    public interface IKangaEstimateProcessor
    {
        EstimatesResponseDto Process(EstimatesApiResponseDto apiResponseData);
    }
}