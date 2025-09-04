using Comvoya.Application.Exceptions;
using ComvoyaAPI.Application.Models;
using ComvoyaAPI.Domain.Entities;
using ComvoyaAPI.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ComvoyaAPI.Services.UserService
{
    public class UserService(AppDbContext context, JwtTokenService jwtTokenService) : IUserService
    {
        public async Task<User?> RegisterAsync(UserDto request)
        {
            if (await context.Users.AnyAsync(u => u.Username == request.Username))
            {
                return null;
            }

            var user = new User();

            var hashedPassword = new PasswordHasher<User>()
                .HashPassword(user, request.Password);

            user.Username = request.Username;
            user.Name = request.Name;
            user.Lastname = request.Lastname;
            user.Email = request.Email;
            user.PasswordHash = hashedPassword;

            context.Users.Add(user);
            await context.SaveChangesAsync();

            return user;
        }
        public async Task<string?> LoginAsync(UserDto request)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);

            if (user == null)
            {
                return null;
            }

            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
            {
                return null;
            }

            string token = jwtTokenService.CreateToken(user);
            return token;
        }

        public async Task<User> GetUserAsync(Guid id)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                throw new UserNotFoundException(id);

            return user;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
                throw new UserNotFoundException(username);

            return user;
        }

        public async Task UpdateUserAsync(UserDto request, CancellationToken ct)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == request.Id);

            if (user == null)
                throw new UserNotFoundException(request.Id);

            if (request.Username != user.Username)
            {
                var usernameExisting = await context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);

                if (usernameExisting != null)
                    throw new UsernameAlreadyExistsException(request.Username);
            }

            user.Username = request.Username;
            user.Name = request.Name;
            user.Email = request.Email;
            user.Lastname = request.Lastname;

            await context.SaveChangesAsync(ct);
        }

        public async Task DeleteUserByIdAsync(Guid id, CancellationToken ct)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                throw new UserNotFoundException(id);

            context.Users.Remove(user);
            await context.SaveChangesAsync(ct);
        }

        public async Task<List<InterestDTO>> GetUserInterestsAsync(Guid id, CancellationToken ct)
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

        public async Task AddUserInterestAsync(Guid userId, int interestId, CancellationToken ct)
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

        public async Task DeleteUserInterestAsync(Guid userId, int interestId, CancellationToken ct)
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
