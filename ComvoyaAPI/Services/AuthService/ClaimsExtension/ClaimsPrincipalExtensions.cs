using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ComvoyaAPI.Services.AuthService.ClaimsExtension
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetUserIdOrThrowUnauthorized(this ClaimsPrincipal user)
        {
            string id = user.FindFirst(JwtRegisteredClaimNames.Sub).Value;

            if (string.IsNullOrWhiteSpace(id))
                throw new UnauthorizedAccessException("User id missing from token.");

            if (!Guid.TryParse(id, out var guid))
                throw new UnauthorizedAccessException("User id missing from token.");

            return guid;
        }
    }
}
