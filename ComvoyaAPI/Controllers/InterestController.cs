using ComvoyaAPI.Application.Models;
using ComvoyaAPI.Domain.Entities;
using ComvoyaAPI.Services.InterestService;
using ComvoyaAPI.Services.UserService;
using Microsoft.AspNetCore.Mvc;

namespace ComvoyaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InterestController(IInterestService interestService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> GetInterests()
        {
            var interests = await interestService.GetInterestsAsync();

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

        [HttpGet("{int:id}")]
        public async Task<ActionResult> GetInterest(int id)
        {
            var interest = await interestService.GetInterestAsync(id);

            if (interest == null)
            {
                {
                    return NotFound(new ProblemDetails
                    {
                        Status = StatusCodes.Status404NotFound,
                        Title = "Interest not found.",
                        Detail = $"Interest with id {id} not found"
                    });
                }
            }

            return Ok(interest);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateInterestAsync(InterestDTO request, CancellationToken ct)
        {
            await interestService.UpdateInterestAsync(request, ct);

            return Ok(new { message = "Interest updated successfully." });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteInterestAsync(int id, CancellationToken ct)
        {
            await interestService.DeleteInterestByIdAsync(id, ct);

            return Ok(new { message = "Interest removed successfully." });
        }
    }
}
