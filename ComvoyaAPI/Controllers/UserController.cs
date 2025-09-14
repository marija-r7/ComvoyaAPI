using Comvoya.Application.Models.User;
using ComvoyaAPI.Domain.Entities;
using ComvoyaAPI.Services.AuthService.ClaimsExtension;
using ComvoyaAPI.Services.InterestService;
using ComvoyaAPI.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ComvoyaAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController(IUserService userService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<UserResponseDTO>>> GetUsers(CancellationToken ct)
        {
            var users = await userService.GetUsers(ct);
            if (users == null)
            {
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Users not found.",
                    Detail = "No users have been found."
                });
            }
            return Ok(users);
        }
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<UserResponseDTO>> GetUser(Guid id)
        {
            var user = await userService.GetUser(id);

            if (user == null)
            {
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "User not found.",
                    Detail = $"User with id {id} not found."
                });
            }

            return Ok(user);
        }
        [HttpGet("me")]
        public async Task<ActionResult<UserResponseDTO>> GetUser()
        {
            var userId = User.GetUserIdOrThrowUnauthorized();
            var user = await userService.GetUser(userId);
            return Ok(user);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> UpdateUserAsync(Guid id, [FromBody] UserUpdateDTO request, CancellationToken ct)
        {
            await userService.UpdateUser(id, request, ct);

            return Ok(new { message = "User updated successfully." });
        }
        [HttpPut("me")]
        public async Task<ActionResult> UpdateMe([FromBody] UserUpdateDTO request, CancellationToken ct)
        {
            var userId = User.GetUserIdOrThrowUnauthorized();
            await userService.UpdateUser(userId, request, ct);
            return Ok(new { message = "User updated successfully." });
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteUserAsync(Guid id, CancellationToken ct)
        {
            await userService.DeleteUser(id, ct);

            return Ok(new { message = "User removed successfully." });
        }

        [HttpGet("me/interests")]
        public async Task<ActionResult> GetUserInterests(CancellationToken ct)
        {
            var userId = User.GetUserIdOrThrowUnauthorized();
            var interests = await userService.GetUserInterests(userId, ct);

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

        [HttpPost("me/interests/{int:interestId}")]
        public async Task<ActionResult> AddInterest(int interestId, CancellationToken ct)
        {
            var userId = User.GetUserIdOrThrowUnauthorized();
            await userService.AddUserInterest(userId, interestId, ct);
            return Ok(new { message = "Interest added successfully." });
        }

        [HttpDelete("me/interests/{int:interestId}")]
        public async Task<ActionResult> DeleteInterest(int interestId, CancellationToken ct)
        {
            var userId = User.GetUserIdOrThrowUnauthorized();
            await userService.DeleteInterest(userId, interestId, ct);
            return Ok(new { message = "Interest removed successfully." });
        }
    }
}
