using Comvoya.Application.Models.Trip;
using Comvoya.Application.Models.TripParticipant;
using Comvoya.Domain.Entities;
using Comvoya.Domain.Helpers;

namespace ComvoyaAPI.Services.TripService
{
    public interface ITripService
    {
        Task<TripDetailsDTO> CreateTrip(Guid ownerId, TripCreateDTO request, CancellationToken ct);
        Task UpdateTrip(Guid tripId, TripUpdateDTO request, CancellationToken ct);
        Task DeleteTrip(Guid tripId, CancellationToken ct);
        Task<TripDetailsDTO> GetTrip(Guid tripId, CancellationToken ct);
        Task<List<TripDetailsDTO>> GetAll(QueryObject query, CancellationToken ct);
        Task<List<TripParticipantDTO>> GetTripParticipants(Guid tripId, CancellationToken ct);
        Task<TripParticipantDTO> CreateTripParticipationRequest(Guid userId, Guid tripId, CancellationToken ct);
        Task<IReadOnlyList<TripParticipantDTO>> GetPendingRequests(Guid tripId, Guid callerId, CancellationToken ct);
        Task<TripParticipantDTO> ApproveParticipant(Guid tripId, Guid targetUserId, Guid callerId, CancellationToken ct);
        Task<TripParticipantDTO> RejectParticipant(Guid tripId, Guid targetUserId, Guid callerId, CancellationToken ct);
        Task<TripParticipantDTO> LeaveTrip(Guid tripId, Guid callerId, CancellationToken ct);
        //Task<List<TripDetailsDTO>> GetTripsByUser(Guid userId, CancellationToken ct);
        //Task<List<TripParticipant>> GetTripsParticipationByUser(Guid userId, CancellationToken ct);
        //Task<List<TripParticipant>> GetTripsParticipationByOwner(Guid userId, CancellationToken ct);
        //Task RequestTripParticipation(Guid userId, Guid tripId, CancellationToken ct);
        //Task<TripParticipant> AcceptTripParticipation(Guid userId, Guid tripId, CancellationToken ct);
    }
}
