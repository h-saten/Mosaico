
namespace Mosaico.Application.Identity.DTOs
{
    public enum LoginResponseTypeDTO
    {
        Succeeded = 1,
        LockedOut = 2,
        RequiresTwoFactor = 3,
        InvalidData = 4,
        Error = 5,
        Deactivated=6
    }
}