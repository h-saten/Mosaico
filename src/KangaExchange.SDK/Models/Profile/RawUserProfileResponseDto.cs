using System.Collections.Generic;

namespace KangaExchange.SDK.Models.Profile
{
    public class RawUserProfileResponseDto
    {
        public string Result { get; set; }
        public string Code { get; set; }
        public UserProfileDto Personal { get; set; }
        public UserCompanyDto Company { get; set; }
        public string Email { get; set; }
        public bool Kyc { get; set; }
        public Dictionary<string, string> Wallet { get; set; }
        public List<RawUserAddressDto> Addresses { get; set; }
    }
}