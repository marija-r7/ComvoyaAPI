using ComvoyaAPI.Domain.Entities;
using Comvoya.Application.Models.Interest;
using Comvoya.Application.Models.User;

namespace ComvoyaAPI.Services.UserService
{
    public interface IUserService
    {
        Task<List<UserResponseDTO>> GetUsers(CancellationToken ct);
        Task<UserResponseDTO> GetUser(Guid id);
        Task UpdateUser(Guid id, UserUpdateDTO request, CancellationToken ct);
        Task DeleteUser(Guid id, CancellationToken ct);
        Task<List<InterestDTO>> GetUserInterests(Guid id, CancellationToken ct);
        Task AddUserInterest(Guid userId, int interestId, CancellationToken ct);
        Task DeleteInterest(Guid userId, int interestId, CancellationToken ct);
    }
}
