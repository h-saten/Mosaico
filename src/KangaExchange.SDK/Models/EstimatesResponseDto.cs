namespace KangaExchange.SDK.Models
{
    public class EstimatesResponseDto
    {
        public bool BuyCodeRequired { get; set; }
        public KangaEstimatesDto Estimates { get; set; }
        public string Result { get; set; }
    }
}