using System.Security.Claims;

namespace Extensions.UsersManagement.Extensions.Identity;

public static class ClaimExtensions
{
    public static string GetClaimValue(this ClaimsPrincipal userClaimsPrincipal, string claimType)
    {
        return userClaimsPrincipal.Claims.FirstOrDefault((Claim x) => x.Type == claimType)?.Value;
    }
    public static Claim GetClaim(this ClaimsPrincipal userClaimsPrincipal, string claimType)
    {
        return userClaimsPrincipal.Claims.FirstOrDefault((Claim x) => x.Type == claimType);
    }
}

