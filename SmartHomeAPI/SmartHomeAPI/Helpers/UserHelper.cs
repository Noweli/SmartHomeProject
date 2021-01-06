using System.Security.Claims;

namespace SmartHomeAPI.Helpers
{
    public static class UserHelper
    {
        public static string GetCurrentUser(this ClaimsPrincipal user) => user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}