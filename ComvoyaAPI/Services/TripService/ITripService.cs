using Comvoya.Application.Models;
using Comvoya.Domain.Entities;

namespace ComvoyaAPI.Services.TripService
{
    public interface ITripService
    {
        Task<Trip> CreateTrip(TripDTO request, CancellationToken ct);
        Task<Trip> GetTrip(Guid id, CancellationToken ct);
        Task<List<Trip>?> GetTrips(CancellationToken ct);
        Task<List<Trip>?> GetTripsByUser(Guid userId, CancellationToken ct);
        Task<List<TripParticipant>?> GetTripsParticipationByUser(Guid userId, CancellationToken ct);
        Task<TripParticipant?> RequestTripParticipation(Guid userId, Guid tripId, CancellationToken ct);
        Task<TripParticipant> AcceptTripParticipation(Guid userId, Guid tripId, CancellationToken ct);
        Task<Trip> UpdateTrip(TripDTO request, CancellationToken ct);
        Task DeleteTrip(Guid id, CancellationToken ct);
    }
}
