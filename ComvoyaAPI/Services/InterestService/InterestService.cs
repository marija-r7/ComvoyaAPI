using Azure.Core;
using Comvoya.Application.Exceptions;
using ComvoyaAPI.Application.Models;
using ComvoyaAPI.Domain.Entities;
using ComvoyaAPI.Infrastructure.Data;
using ComvoyaAPI.Services.UserService;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace ComvoyaAPI.Services.InterestService
{
    public class InterestService(AppDbContext context) : IInterestService
    {
        public async Task<List<Interest>> GetInterestsAsync()
        {
            return await context.Interests.ToListAsync();
        }
        public async Task<Interest> GetInterestAsync(int id)
        {
            var interest = await context.Interests.FirstOrDefaultAsync(i => i.Id == id);

            if (interest == null)
                throw new InterestNotFoundException(id);

            return interest;
        }
        public async Task UpdateInterestAsync(InterestDTO request, CancellationToken ct)
        {
            var interest = await context.Interests.FirstOrDefaultAsync(i => i.Id == request.Id);

            if (interest == null)
                throw new InterestNotFoundException(request.Id);

            if (request.Name != interest.Name)
            {
                var interestExisting = await context.Interests.FirstOrDefaultAsync(u => u.Name == request.Name);

                if (interestExisting != null)
                    throw new InterestAlreadyExistsException(request.Name!);
            }

            interest.Name = request.Name;

            await context.SaveChangesAsync(ct);
        }
        public async Task DeleteInterestByIdAsync(int id, CancellationToken ct)
        {
            var interest = context.Interests.FirstOrDefault(i => i.Id == id);

            if (interest == null)
                throw new InterestNotFoundException(id);

            context.Interests.Remove(interest);
            await context.SaveChangesAsync(ct);
        }
    }
}
