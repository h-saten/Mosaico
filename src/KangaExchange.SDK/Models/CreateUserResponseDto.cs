namespace KangaExchange.SDK.Models
{
    public class CreateUserResponseDto
    {
        public string Result { get; set; }
        public string AppUserId { get; set; }
        public string Code { get; set; }
        public string PasswordUrl { get; set; }
    }
}