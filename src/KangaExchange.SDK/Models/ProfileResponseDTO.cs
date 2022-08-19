namespace KangaExchange.SDK.Models
{
    public class ProfileResponseDTO
    {
        public string Result { get; set; }
        public string Email { get; set; }
        public UserProfileDTO Personal { get; set; }
        public UserCompanyDTO Company { get; set; }
        public string Code { get; set; }
    }
}