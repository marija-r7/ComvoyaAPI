using Azure.Core;
using Comvoya.Application.Exceptions;
using Comvoya.Application.Models.Trip;
using Comvoya.Application.Models.TripParticipant;
using Comvoya.Application.Models.User;
using Comvoya.Domain.Entities;
using Comvoya.Domain.Enums;
using Comvoya.Domain.Helpers;
using ComvoyaAPI.Domain.Entities;
using ComvoyaAPI.Infrastructure.Data;
using ComvoyaAPI.Services.UserService;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ComvoyaAPI.Services.TripService
{
    public class TripService(AppDbContext context) : ITripService
    {
        public async Task<TripDetailsDTO> CreateTrip(Guid ownerId, TripCreateDTO request, CancellationToken ct)
        {
            bool conflict = await context.Trips
                 .AsNoTracking()
                 .AnyAsync(t =>
                    t.OwnerId == ownerId &&
                    t.LocationId == request.LocationId &&
                    t.DateFromUtc.Date <= request.DateToUtc.Date &&
                    request.DateFromUtc.Date <= t.DateToUtc.Date,
                    ct);

            if (conflict)
                throw new TripOnThisDateAlreadyExistsException(request.DateFromUtc, request.DateToUtc);

            var trip = new Trip
            {
                Title = request.Title,
                Description = request.Description,
                MaxParticipants = request.MaxParticipants,
                Budget = request.Budget,
                DateFromUtc = request.DateFromUtc,
                DateToUtc = request.DateToUtc,
                OwnerId = ownerId,
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow

            };
            trip.Participants.Add(new TripParticipant()
            {
                TripId = trip.Id,
                UserId = trip.OwnerId,
                Role = TripParticipantRole.Host,
                Status = TripParticipantStatus.Joined,
                CreatedAt = DateTime.UtcNow
            });

            context.Trips.Add(trip);
            await context.SaveChangesAsync(ct);

            return new TripDetailsDTO
            {
                Id = trip.Id,
                Title = trip.Title,
                Description = trip.Description,
                DateFromUtc = trip.DateFromUtc,
                DateToUtc = trip.DateToUtc,
                LocationId = trip.LocationId,
                Budget = trip.Budget,
                MaxParticipants = trip.MaxParticipants
            };
        }
        public async Task UpdateTrip(Guid tripId, TripUpdateDTO request, CancellationToken ct)
        {
            var trip = await context.Trips.FirstOrDefaultAsync(x => x.Id == tripId);

            if (trip == null)
                throw new TripNotFoundException(tripId);

            if (request.Title != null) trip.Title = request.Title.Trim();
            if (request.Title is not null) trip.Title = request.Title.Trim();
            if (request.Description is not null) trip.Description = request.Description;
            if (request.LocationId.HasValue) trip.LocationId = request.LocationId.Value;
            if (request.Budget.HasValue) trip.Budget = request.Budget;
            if (request.MaxParticipants.HasValue)
            {
                var acceptedCount = trip.Participants.Count(p => p.Status == TripParticipantStatus.Joined);
                if (request.MaxParticipants.Value < acceptedCount)
                    throw new ValidationException($"MaxParticipants ({request.MaxParticipants}) < current joined ({acceptedCount}).");
                trip.MaxParticipants = request.MaxParticipants.Value;
            }
            if (request.DateFromUtc.HasValue || request.DateToUtc.HasValue)
            {
                var from = request.DateFromUtc ?? trip.DateFromUtc;
                var to = request.DateToUtc ?? trip.DateToUtc;

                if (from > to)
                    throw new ValidationException("DateFromUtc cannot be after DateToUtc.");

                trip.DateFromUtc = from;
                trip.DateToUtc = to;
            }

            trip.LastUpdatedAt = DateTime.UtcNow;

            await context.SaveChangesAsync(ct);
        }

        public async Task DeleteTrip(Guid tripId, CancellationToken ct)
        {
            var trip = await context.Trips.FirstOrDefaultAsync(x => x.Id == tripId);

            if (trip == null)
                throw new TripNotFoundException(tripId);

            context.Remove(trip);
            await context.SaveChangesAsync(ct);
        }

        public async Task<TripDetailsDTO> GetTrip(Guid tripId, CancellationToken ct)
        {
            var trip = await context.Trips.FirstOrDefaultAsync(x => x.Id == tripId);

            return new TripDetailsDTO
            {
                Id = trip.Id,
                Title = trip.Title,
                Description = trip.Description,
                DateFromUtc = trip.DateFromUtc,
                DateToUtc = trip.DateToUtc,
                LocationId = trip.LocationId,
                Budget = trip.Budget,
                MaxParticipants = trip.MaxParticipants
            };
        }

        public async Task<List<TripDetailsDTO>> GetAll(QueryObject query, CancellationToken ct)
        {
            var q = context.Trips.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Title))
            {
                var term = query.Title.Trim();
                q = q.Where(t => EF.Functions.Like(t.Title, $"%{term}%"));
            }

            if (query.Budget.HasValue)
                q = q.Where(t => t.Budget == query.Budget.Value);

            if (query.LocationId.HasValue)
                q = q.Where(t => t.LocationId == query.LocationId.Value);

            if (query.OwnerId.HasValue)
                q = q.Where(t => t.OwnerId == query.OwnerId.Value);

            if (query.DateFromUtc.HasValue && query.DateToUtc.HasValue)
            {
                var from = query.DateFromUtc.Value;
                var to = query.DateToUtc.Value;
                q = q.Where(t => t.DateFromUtc <= to && t.DateToUtc >= from);
            }
            else if (query.DateFromUtc.HasValue)
            {
                var from = query.DateFromUtc.Value;
                q = q.Where(t => t.DateToUtc >= from);
            }
            else if (query.DateToUtc.HasValue)
            {
                var to = query.DateToUtc.Value;
                q = q.Where(t => t.DateFromUtc <= to);
            }

            var dtoQuery = q.Select(t => new TripDetailsDTO
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                DateFromUtc = t.DateFromUtc,
                DateToUtc = t.DateToUtc,
                Owner = new UserSummaryDTO
                {
                    Id = t.Owner.Id,
                    Username = t.Owner.Username,
                    Name = t.Owner.Name,
                    Lastname = t.Owner.Lastname
                },
                MaxParticipants = t.MaxParticipants,
                Budget = t.Budget,
                LocationId = t.LocationId,
            });

            return await dtoQuery.ToListAsync(ct);
        }

        public async Task<List<TripParticipantDTO>> GetTripParticipants(Guid tripId, CancellationToken ct)
        {
            var participants = await context.TripParticipants
                                            .AsNoTracking()
                                            .Where(p => p.TripId == tripId)
                                            .Select(p => new TripParticipantDTO
                                            {
                                                UserId = p.UserId,
                                                Role = p.Role,
                                                Status = p.Status
                                            })
                                            .ToListAsync(ct);
            return participants;
        }

        public async Task<TripParticipantDTO> CreateTripParticipationRequest(Guid callerId, Guid tripId, CancellationToken ct)
        {
            var tripExists = await context.Trips.AnyAsync(t => t.Id == tripId, ct);
            if (!tripExists) throw new KeyNotFoundException("Trip not found.");

            var participant = await context.TripParticipants
                .FirstOrDefaultAsync(p => p.TripId == tripId && p.UserId == callerId, ct);

            if (participant is not null)
            {
                switch (participant.Status)
                {
                    case TripParticipantStatus.Pending:
                        break;

                    case TripParticipantStatus.Joined:
                        throw new InvalidOperationException("Already a participant.");

                    case TripParticipantStatus.Rejected:
                    case TripParticipantStatus.Left:
                        participant.Status = TripParticipantStatus.Pending;
                        participant.CreatedAt = DateTime.UtcNow;
                        await context.SaveChangesAsync(ct);
                        break;

                    default:
                        throw new InvalidOperationException("Invalid participant state.");
                }

                var userDto = await GetUserDto(callerId, ct);
                return new TripParticipantDTO
                {
                    UserId = callerId,
                    User = userDto,
                    Role = participant.Role,
                    Status = participant.Status
                };
            }

            var entity = new TripParticipant
            {
                TripId = tripId,
                UserId = callerId,
                Role = TripParticipantRole.Guest,
                Status = TripParticipantStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            context.TripParticipants.Add(entity);
            await context.SaveChangesAsync(ct);

            var createdUserDto = await GetUserDto(callerId, ct);
            return new TripParticipantDTO
            {
                UserId = callerId,
                User = createdUserDto,
                Role = entity.Role,
                Status = entity.Status
            };
        }
        public async Task<IReadOnlyList<TripParticipantDTO>> GetPendingRequests(Guid tripId, Guid callerId, CancellationToken ct)
        {
            await EnsureOrganizerAsync(tripId, callerId, ct);

            return await context.TripParticipants
                .Where(p => p.TripId == tripId && p.Status == TripParticipantStatus.Pending)
                .Select(p => new TripParticipantDTO
                {
                    UserId = p.UserId,
                    Role = p.Role,
                    Status = p.Status,
                    User = new UserResponseDTO
                    {
                        Id = p.User.Id,
                        Username = p.User.Username,
                        Name = p.User.Name,
                        Lastname = p.User.Lastname
                    }
                })
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public async Task<TripParticipantDTO> ApproveParticipant(Guid tripId, Guid targetUserId, Guid callerId, CancellationToken ct)
        {
            await EnsureOrganizerAsync(tripId, callerId, ct);

            var trip = await context.Trips
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == tripId, ct)
                ?? throw new KeyNotFoundException("Trip not found.");

            var participant = await context.TripParticipants
                .FirstOrDefaultAsync(p => p.TripId == tripId && p.UserId == targetUserId, ct)
                ?? throw new KeyNotFoundException("Join request not found.");

            if (participant.Status != TripParticipantStatus.Pending)
                throw new InvalidOperationException("Only pending requests can be approved.");

            if (trip.MaxParticipants != 0)
            {
                var approvedCount = await context.TripParticipants
                    .CountAsync(p => p.TripId == tripId && p.Status == TripParticipantStatus.Joined, ct);

                var seatsUsed = approvedCount + 1;
                if (seatsUsed >= trip.MaxParticipants)
                    throw new InvalidOperationException("Trip is full.");
            }

            participant.Status = TripParticipantStatus.Joined;
            await context.SaveChangesAsync(ct);

            return await GetParticipantDtoAsync(tripId, targetUserId, ct);
        }

        public async Task<TripParticipantDTO> RejectParticipant(Guid tripId, Guid targetUserId, Guid callerId, CancellationToken ct)
        {
            await EnsureOrganizerAsync(tripId, callerId, ct);

            var participant = await context.TripParticipants
                .FirstOrDefaultAsync(p => p.TripId == tripId && p.UserId == targetUserId, ct)
                ?? throw new KeyNotFoundException("Join request not found.");

            if (participant.Status != TripParticipantStatus.Pending)
                throw new InvalidOperationException("Only pending requests can be rejected.");

            participant.Status = TripParticipantStatus.Rejected;
            await context.SaveChangesAsync(ct);

            return await GetParticipantDtoAsync(tripId, targetUserId, ct);
        }

        public async Task<TripParticipantDTO> LeaveTrip(Guid tripId, Guid callerId, CancellationToken ct)
        {
            var isOwner = await context.Trips.AnyAsync(t => t.Id == tripId && t.OwnerId == callerId, ct);
            if (isOwner)
                throw new InvalidOperationException("Owner cannot leave their own trip.");

            var participant = await context.TripParticipants
                .FirstOrDefaultAsync(p => p.TripId == tripId && p.UserId == callerId, ct);

            if (participant is null)
                throw new KeyNotFoundException("You are not a participant of this trip.");

            if (participant.Status == TripParticipantStatus.Left)
                return await GetParticipantDtoAsync(tripId, callerId, ct);

            participant.Status = TripParticipantStatus.Left;
            await context.SaveChangesAsync(ct);

            return await GetParticipantDtoAsync(tripId, callerId, ct);
        }

        private async Task EnsureOrganizerAsync(Guid tripId, Guid userId, CancellationToken ct)
        {
            var isOwner = await context.Trips.AnyAsync(t => t.Id == tripId && t.OwnerId == userId, ct);
            if (isOwner) return;

            var isCoHost = await context.TripParticipants.AnyAsync(p =>
                p.TripId == tripId &&
                p.UserId == userId &&
                p.Status == TripParticipantStatus.Joined &&
                p.Role == TripParticipantRole.CoHost, ct);

            if (!isCoHost)
                throw new UnauthorizedAccessException("Only host/co-host can perform this action.");
        }

        private async Task<TripParticipantDTO> GetParticipantDtoAsync(Guid tripId, Guid userId, CancellationToken ct)
        {
            return await context.TripParticipants
                .Where(p => p.TripId == tripId && p.UserId == userId)
                .Select(p => new TripParticipantDTO
                {
                    UserId = p.UserId,
                    Role = p.Role,
                    Status = p.Status,
                    User = new UserResponseDTO
                    {
                        Id = p.User.Id,
                        Username = p.User.Username,
                        Name = p.User.Name,
                        Lastname = p.User.Lastname
                    }
                })
                .AsNoTracking()
                .SingleAsync(ct);
        }
        private async Task<UserResponseDTO> GetUserDto(Guid userId, CancellationToken ct) =>
            await context.Users.AsNoTracking()
                .Where(u => u.Id == userId)
                .Select(u => new UserResponseDTO
                {
                    Id = u.Id,
                    Username = u.Username,
                    Name = u.Name,
                    Lastname = u.Lastname
                })
                .SingleAsync(ct);
    }
}
