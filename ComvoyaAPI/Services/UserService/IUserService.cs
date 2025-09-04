using ComvoyaAPI.Domain.Entities;
using ComvoyaAPI.Application.Models;

namespace ComvoyaAPI.Services.UserService
{
    public interface IUserService
    {
        Task<User?> RegisterAsync(UserDto request);
        Task<string?> LoginAsync(UserDto request);
        Task<User> GetUserAsync(Guid id);
        Task<User> GetUserByUsernameAsync(string username);
        Task UpdateUserAsync(UserDto request, CancellationToken ct);
        Task DeleteUserByIdAsync(Guid id, CancellationToken ct);
        Task<List<InterestDTO>> GetUserInterestsAsync(Guid id, CancellationToken ct);
        Task AddUserInterestAsync(Guid userId, int interestId, CancellationToken ct);
        Task DeleteUserInterestAsync(Guid userId, int interestId, CancellationToken ct);
    }
}
