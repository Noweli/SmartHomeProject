using SmartHomeAPI.Entity;

namespace SmartHomeAPI.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}