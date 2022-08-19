namespace KangaExchange.SDK.Models.Profile
{
    public class UserAddressDto
    {
        public string Currency { get; set; }
        public string Address { get; set; }
        public bool IsLegacy { get; set; }

        public UserAddressDto(string currency, string address, bool isLegacy = false)
        {
            Currency = currency;
            Address = address;
            IsLegacy = isLegacy;
        }
    }
}