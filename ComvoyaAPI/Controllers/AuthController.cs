using Comvoya.Application.Models.User;
using ComvoyaAPI.Domain.Entities;
using ComvoyaAPI.Services.AuthService;
using ComvoyaAPI.Services.AuthService.ClaimsExtension;
using ComvoyaAPI.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComvoyaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(AuthService authService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserRegisterDTO request)
        {
            var user = await authService.RegisterAsync(request);
            if (user == null)
            {
                return BadRequest("Username already exists.");
            }
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserLoginDTO request)
        {
            var token = await authService.LoginAsync(request);
            if (token == null)
            {
                return BadRequest("Invalid username or password.");
            }
            return Ok(token);
        }

        //[Authorize]
        //[HttpPost("logout")]
        //public async Task<IActionResult> Logout(CancellationToken ct)
        //{
        //    var userId = User.GetUserIdOrThrowUnauthorized();
        //    return null;
        //}
    }
}
