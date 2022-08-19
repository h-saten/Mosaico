namespace Mosaico.Application.KangaWallet.DTOs
{
    public class KangaAssetDto
    {
        public string Symbol { get; set; }
        public decimal Balance { get; set; }
        public decimal TotalAssetValue { get; set; }
        public string Currency { get; set; }
    }
}