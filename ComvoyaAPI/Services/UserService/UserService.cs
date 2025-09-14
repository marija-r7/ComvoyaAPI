using Comvoya.Application.Exceptions;
using Comvoya.Application.Models.Interest;
using Comvoya.Application.Models.User;
using Comvoya.Application.Models.UserInterest;
using ComvoyaAPI.Domain.Entities;
using ComvoyaAPI.Infrastructure.Data;
using ComvoyaAPI.Services.AuthService.JwtToken;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ComvoyaAPI.Services.UserService
{
    public class UserService(AppDbContext context) : IUserService
    {
        public async Task<List<UserResponseDTO>> GetUsers(CancellationToken ct)
        {
            var users = await context.Users
                .AsNoTracking()
                .OrderBy(u => u.Username)
                .Select(u => new UserResponseDTO
                {
                    Id = u.Id,
                    Name = u.Name,
                    Lastname = u.Lastname,
                    Username = u.Username,
                    Email = u.Email,
                    UserInterests = u.UserInterests
                                .OrderBy(i => i.Name)
                                .Select(i => new UserInterestDTO
                                {
                                    UserId = u.Id,
                                    InterestId = i.InterestId,
                                    Name = i.Name
                                }).ToList()
                })
                .ToListAsync(ct);
            return users;
        }
        public async Task<UserResponseDTO> GetUser(Guid id)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                throw new UserNotFoundException(id);

            return new UserResponseDTO
            {
                Id = user.Id,
                Name = user.Name,
                Lastname = user.Lastname,
                Email = user.Email,
                Username = user.Username
            };
        }

        public async Task UpdateUser(Guid id, UserUpdateDTO request, CancellationToken ct)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                throw new UserNotFoundException(id);

            if (request.Username != user.Username)
            {
                var usernameExisting = await context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);

                if (usernameExisting != null)
                    throw new UsernameAlreadyExistsException(request.Username);
            }

            if (request.Username != null) user.Username = request.Username;
            user.Name = request.Name;
            user.Email = request.Email;
            user.Lastname = request.Lastname;

            await context.SaveChangesAsync(ct);
        }

        public async Task DeleteUser(Guid id, CancellationToken ct)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                throw new UserNotFoundException(id);

            context.Users.Remove(user);
            await context.SaveChangesAsync(ct);
        }

        public async Task<List<InterestDTO>> GetUserInterests(Guid id, CancellationToken ct)
        {
            var userInterests = await context.UserInterests
                                            .Where(ui => ui.UserId == id)
                                            .Select(ui => new InterestDTO
                                            {
                                                Id = ui.InterestId,
                                                Name = ui.Name
                                            }).ToListAsync(ct);

            if (userInterests == null)
                throw new UserInterestNotFoundException(id);

            return userInterests;
        }

        public async Task AddUserInterest(Guid userId, int interestId, CancellationToken ct)
        {
            bool interestExists = await context.UserInterests.AnyAsync(ui => ui.UserId == userId && ui.InterestId == interestId, ct);

            if (!interestExists)
            {
                var interest = await context.Interests.FirstOrDefaultAsync(i => i.Id == interestId);
                var userInterest = new UserInterest { InterestId = interestId, UserId = userId, Name = interest!.Name };
                context.UserInterests.Add(userInterest);
                await context.SaveChangesAsync(ct);
            }
        }

        public async Task DeleteInterest(Guid userId, int interestId, CancellationToken ct)
        {
            var userInterest = await context.UserInterests.FirstOrDefaultAsync(ui => ui.UserId == userId && ui.InterestId == interestId, ct);

            if (userInterest != null)
            {
                context.UserInterests.Remove(userInterest);
                await context.SaveChangesAsync(ct);
            }
        }
    }
}
