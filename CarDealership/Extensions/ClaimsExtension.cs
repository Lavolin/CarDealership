using System.Security.Claims;

namespace CarDealership.Extensions
{
    public static class ClaimsExtension
    {
        public static string Id(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
