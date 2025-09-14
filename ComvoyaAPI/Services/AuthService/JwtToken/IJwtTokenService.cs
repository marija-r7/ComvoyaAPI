using ComvoyaAPI.Domain.Entities;

namespace ComvoyaAPI.Services.AuthService.JwtToken
{
    public interface IJwtTokenService
    {
        string CreateToken(User user);
    }
}
