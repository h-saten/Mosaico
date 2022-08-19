namespace KangaExchange.SDK.Models.Profile
{
    public class UserWalletEntryDto
    {
        public string Currency { get; set; }
        public string Amount { get; set; }
        public bool IeoToken { get; set; }

        public UserWalletEntryDto(string currency, string amount, bool ieoToken = false)
        {
            Currency = currency;
            Amount = amount;
            IeoToken = ieoToken;
        }
    }
}