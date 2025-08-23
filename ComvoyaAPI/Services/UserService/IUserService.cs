using ComvoyaAPI.Domain.Entities;
using ComvoyaAPI.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace ComvoyaAPI.Services.UserService
{
    public interface IUserService
    {
        Task<User> RegisterAsync(UserDto request);
        Task<string> LoginAsync(UserDto request);
        Task<User> GetUserAsync(Guid id);
        Task<User> GetUserByUsernameAsync(string username);
        Task UpdateUserAsync(UserDto request);
        Task DeleteUserByIdAsync(Guid id);
    }
}
