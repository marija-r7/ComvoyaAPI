using ComvoyaAPI.Application.Models;
using ComvoyaAPI.Domain.Entities;
using ComvoyaAPI.Services.InterestService;
using ComvoyaAPI.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ComvoyaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService userService) : ControllerBase
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
        [HttpGet("{id:guid}")]
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


        [HttpGet("{string: username}")]
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
        public async Task<IActionResult> UpdateUserAsync(UserDto request, CancellationToken ct)
        {
            await userService.UpdateUserAsync(request, ct);

            return Ok(new { message = "User updated successfully." });
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteUserAsync(Guid id, CancellationToken ct)
        {
            await userService.DeleteUserByIdAsync(id, ct);

            return Ok(new { message = "User removed successfully." });
        }

        [HttpGet("{id:guid}/interests")]
        public async Task<ActionResult> GetUserInterests(Guid id, CancellationToken ct)
        {
            var interests = await userService.GetUserInterestsAsync(id, ct);

            if (interests == null)
            {
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Interests not found.",
                    Detail = $"No interests have been found in the database."
                });
            }

            return Ok(interests);
        }

        [HttpPost("{userId:guid}/addInterest/{int:interestId}")]
        public async Task<IActionResult> AddUserInterest(Guid userId, int interestId, CancellationToken ct)
        {
            await userService.AddUserInterestAsync(userId, interestId, ct);

            return Ok(new { message = "Interest added successfully." });
        }

        [HttpDelete("{userId:guid}/deleteInterest/{int:interestId}")]
        public async Task<IActionResult> DeleteUserInterest(Guid userId, int interestId, CancellationToken ct)
        {
            await userService.DeleteUserInterestAsync(userId, interestId, ct);

            return Ok(new { message = "Interest removed successfully." });
        }
    }
}
