namespace Mosaico.SDK.Identity.Models
{
    public class CreateUserRequest
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ExternalId { get; set; }
        public string Username { get; set; }
    }
}