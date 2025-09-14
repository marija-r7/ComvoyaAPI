using Comvoya.Domain.Entities;
using ComvoyaAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ComvoyaAPI.Infrastructure.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Interest> Interests { get; set; }
        public DbSet<UserInterest> UserInterests { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<TripParticipant> TripParticipants { get; set; }
    }
}
