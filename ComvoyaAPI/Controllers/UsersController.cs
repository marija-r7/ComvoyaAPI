using ComvoyaAPI.Domain.Entities;
using ComvoyaAPI.Application.Models;
using ComvoyaAPI.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ComvoyaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IUserService userService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            var user = await userService.RegisterAsync(request);
            if (user == null)
            {
                return BadRequest("Username already exists.");
            }
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDto request)
        {
            var token = await userService.LoginAsync(request);
            if (token == null)
            {
                return BadRequest("Invalid username or password.");
            }
            return Ok(token);
        }
        [HttpGet("byId/{id:guid}")]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            var user = await userService.GetUserAsync(id);

            if (user == null)
            {
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "User not found.",
                    Detail = $"User with id {id} not found"
                });
            }

            return Ok(user);
        }


        [HttpGet("byUsername/{username}")]
        public async Task<ActionResult<User>> GetUserByUsername(string username)
        {
            var user = await userService.GetUserByUsernameAsync(username);

            if (user == null)
            {
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "User not found.",
                    Detail = $"User with username {username} not found"
                });
            }

            return Ok(user);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateUserAsync(UserDto request)
        {
            await userService.UpdateUserAsync(request);

            return Ok(new { message = "User updated successfully." });
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteUserAsync(Guid id)
        {
            await userService.DeleteUserByIdAsync(id);

            return Ok(new { message = "User removed successfully." });
        }
    }
}
