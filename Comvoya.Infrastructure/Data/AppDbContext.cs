using ComvoyaAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ComvoyaAPI.Infrastructure.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
    }
}
