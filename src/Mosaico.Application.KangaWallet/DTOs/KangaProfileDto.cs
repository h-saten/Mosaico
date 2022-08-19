using System.Collections.Generic;
using KangaExchange.SDK.Models.Profile;

namespace Mosaico.Application.KangaWallet.DTOs
{
    public class KangaProfileDto
    {
        public UserProfileDto PersonalData { get; set; }
        public UserCompanyDto Company { get; set; }
        public List<UserWalletEntryDto> Wallet { get; set; }
        public List<UserAddressDto> Addresses { get; set; }
        public bool ApprovedKyc { get; set; }
        public UserKangaKycStatusDto KycStatus { get; set; }
    }
    
    public enum UserKangaKycStatusDto
    {
        Accepted = 1,
        Rejected = 2
    }
}