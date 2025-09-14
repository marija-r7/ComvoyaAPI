using Comvoya.Application.Exceptions;
using Comvoya.Application.Models.User;
using Comvoya.Domain.Enums;
using ComvoyaAPI.Domain.Entities;
using ComvoyaAPI.Infrastructure.Data;
using ComvoyaAPI.Services.AuthService.JwtToken;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ComvoyaAPI.Services.AuthService
{
    public class AuthService(AppDbContext context) : IAuthService
    {
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IJwtTokenService _jwtTokenService;
        public async Task<UserResponseDTO> RegisterAsync(UserRegisterDTO request)
        {
            if (await context.Users.AnyAsync(u => u.Username == request.Username))
            {
                throw new UsernameAlreadyExistsException(request.Username);
            }

            var user = new User
            {
                Username = request.Username,
                Name = request.Name,
                Lastname = request.Lastname,
                Email = request.Email,
                Role = UserRole.Standard
            };

            user.setPasswordHash(_passwordHasher.HashPassword(user, request.Password));

            context.Users.Add(user);
            await context.SaveChangesAsync();

            return new UserResponseDTO
            {
                Id = user.Id,
                Name = user.Name,
                Lastname = user.Lastname,
                Email = user.Email,
                Username = user.Username
            };
        }
        public async Task<string> LoginAsync(UserLoginDTO request)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Username == request.UsernameOrEmail || u.Email == request.UsernameOrEmail);

            if (user == null)
            {
                throw new UserNotFoundException(request.UsernameOrEmail);
            }

            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
            {
                throw new UserLoginFailedException();
            }

            string token = _jwtTokenService.CreateToken(user);
            return token;
        }
        public async Task Logout(CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
