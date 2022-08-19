namespace Mosaico.Application.Identity.DTOs
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class LoginResponseDTO
    {
        public readonly LoginResult result;

        public LoginResponseDTO(LoginResult result = null)
        {
            this.result = result;
        }
    }
}