using System.Collections.Generic;

namespace KangaExchange.SDK.Models.Profile
{
    public class UserProfileResponseDto
    {
        public string Result { get; set; }
        public string Code { get; set; }
        public UserProfileDto Personal { get; set; }
        public UserCompanyDto Company { get; set; }
        public string Email { get; set; }
        public bool Kyc { get; set; }
        public List<UserWalletEntryDto> Wallet { get; set; }
        public List<UserAddressDto> Addresses { get; set; }
    }
}