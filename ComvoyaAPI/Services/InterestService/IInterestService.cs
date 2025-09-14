using Comvoya.Application.Models.Interest;
using ComvoyaAPI.Domain.Entities;

namespace ComvoyaAPI.Services.InterestService
{
    public interface IInterestService
    {
        Task<List<Interest>> GetInterestsAsync();
        Task<Interest> GetInterestAsync(int id);
        Task UpdateInterestAsync(InterestDTO request, CancellationToken ct);
        Task DeleteInterestByIdAsync(int id, CancellationToken ct);
    }
}
