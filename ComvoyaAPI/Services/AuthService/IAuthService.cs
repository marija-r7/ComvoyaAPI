using Comvoya.Application.Models.User;
using ComvoyaAPI.Domain.Entities;

namespace ComvoyaAPI.Services.AuthService
{
    public interface IAuthService
    {
        Task<UserResponseDTO> RegisterAsync(UserRegisterDTO request);
        Task<string> LoginAsync(UserLoginDTO request);
        Task Logout(CancellationToken ct);
    }
}
