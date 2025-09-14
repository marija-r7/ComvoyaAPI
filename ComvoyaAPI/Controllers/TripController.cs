using Comvoya.Application.Models.Trip;
using Comvoya.Application.Models.TripParticipant;
using Comvoya.Domain.Entities;
using Comvoya.Domain.Helpers;
using ComvoyaAPI.Services.AuthService.ClaimsExtension;
using ComvoyaAPI.Services.TripService;
using Microsoft.AspNetCore.Mvc;

namespace ComvoyaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripController(ITripService tripService) : Controller
    {
        [HttpPost]
        public async Task<ActionResult<TripDetailsDTO>> CreateTrip(TripCreateDTO request, CancellationToken ct)
        {
            var userId = User.GetUserIdOrThrowUnauthorized();
            var trip = await tripService.CreateTrip(userId, request, ct);

            return CreatedAtAction(nameof(GetTrip), new { id = trip.Id }, trip);
        }
        [HttpGet]
        public async Task<ActionResult<List<TripDetailsDTO>>> GetAll([FromBody] QueryObject query, CancellationToken ct)
        {
            var trips = await tripService.GetAll(query, ct);
            return Ok(trips);
        }

        [HttpGet("{tripId:guid}")]
        public async Task<ActionResult<TripDetailsDTO>> GetTrip(Guid tripId, CancellationToken ct)
        {
            var trip = await tripService.GetTrip(tripId, ct);

            return trip == null ? NotFound() : Ok(trip);
        }

        [HttpPut("{tripId:guid}")]
        public async Task<ActionResult> UpdateTrip(Guid tripId, TripUpdateDTO request, CancellationToken ct)
        {
            await tripService.UpdateTrip(tripId, request, ct);

            return Ok(new { message = "Trip updated successfully." });
        }

        [HttpDelete("{tripId:guid}")]
        public async Task<ActionResult> DeleteTrip(Guid tripId, CancellationToken ct)
        {
            await tripService.DeleteTrip(tripId, ct);

            return Ok(new { message = "Trip removed successfully." });
        }

        [HttpGet("{tripId:guid}/participants")]
        public async Task<ActionResult<List<TripParticipantDTO>>> GetTripParticipants(Guid tripId, CancellationToken ct)
        {
            var participants = await tripService.GetTripParticipants(tripId, ct);

            return participants != null ? Ok(participants) : NotFound();
        }

        [HttpPost("{tripId:guid}/participants/request")]
        public async Task<ActionResult<TripParticipantDTO>> RequestParticipation(Guid tripId, CancellationToken ct)
        {
            var callerId = User.GetUserIdOrThrowUnauthorized();
            var dto = await tripService.CreateTripParticipationRequest(tripId, callerId, ct);
            return Accepted(dto);
        }

        [HttpGet("{tripId:guid}/participants/pending")]
        public async Task<ActionResult<IReadOnlyList<TripParticipantDTO>>> GetPending(Guid tripId, CancellationToken ct)
        {
            var callerId = User.GetUserIdOrThrowUnauthorized();
            var list = await tripService.GetPendingRequests(tripId, callerId, ct);
            return Ok(list);
        }
        [HttpPost("{tripId:guid}/participants/{userId:guid}/approve")]
        public async Task<ActionResult<TripParticipantDTO>> Approve(Guid tripId, Guid userId, CancellationToken ct)
        {
            var callerId = User.GetUserIdOrThrowUnauthorized();
            var dto = await tripService.ApproveParticipant(tripId, userId, callerId, ct);
            return Ok(dto);
        }
        [HttpPost("{tripId:guid}/participants/{userId:guid}/reject")]
        public async Task<ActionResult<TripParticipantDTO>> Reject(Guid tripId, Guid userId, CancellationToken ct)
        {
            var callerId = User.GetUserIdOrThrowUnauthorized();
            var dto = await tripService.RejectParticipant(tripId, userId, callerId, ct);
            return Ok(dto);
        }
        [HttpDelete("{tripId:guid}/participants/me")]
        public async Task<ActionResult<TripParticipantDTO>> Leave(Guid tripId, CancellationToken ct)
        {
            var callerId = User.GetUserIdOrThrowUnauthorized();
            var dto = await tripService.LeaveTrip(tripId, callerId, ct);
            return Ok(dto);
        }
    }
}
