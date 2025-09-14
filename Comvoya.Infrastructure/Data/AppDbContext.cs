using Comvoya.Domain.Entities;
using Comvoya.Infrastructure.Data.Configurations;
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
        public DbSet<TripParticipant> TripParticipants { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(e =>
            {
                e.ToTable("Users");
                e.HasKey(u => u.Id);
                e.Property(u => u.Username).IsRequired().HasMaxLength(64);
                e.Property(u => u.Email).IsRequired().HasMaxLength(256);
                e.HasIndex(u => u.Username).IsUnique();
                e.HasIndex(u => u.Email).IsUnique();
            });

            modelBuilder.Entity<Interest>(e =>
            {
                e.ToTable("Interests");
                e.HasKey(i => i.Id);
                e.Property(i => i.Name).IsRequired().HasMaxLength(64);
                e.HasIndex(i => i.Name).IsUnique();
            });

            modelBuilder.Entity<Trip>(e =>
            {
                e.ToTable("Trips");
                e.HasKey(t => t.Id);

                e.HasOne(t => t.Owner)
                 .WithMany(u => u.OwnedTrips)
                 .HasForeignKey(t => t.OwnerId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<TripParticipant>(e =>
            {
                e.ToTable("TripParticipants");
                e.HasKey(tp => new { tp.TripId, tp.UserId });

                e.Property(tp => tp.Role).HasConversion<int>();
                e.Property(tp => tp.Status).HasConversion<int>();
                e.Property(tp => tp.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");

                e.HasOne(tp => tp.Trip)
                 .WithMany(t => t.Participants)
                 .HasForeignKey(tp => tp.TripId);

                e.HasOne(tp => tp.User)
                 .WithMany(u => u.Trips)
                 .HasForeignKey(tp => tp.UserId);

                e.HasIndex(tp => tp.UserId);
            });

            modelBuilder.Entity<UserInterest>(e =>
            {
                e.ToTable("UserInterests");
                e.HasKey(ui => new { ui.UserId, ui.InterestId });

                e.Property(ui => ui.Name).IsRequired().HasMaxLength(64);

                e.HasIndex(ui => new { ui.UserId, ui.Name })
                 .IsUnique()
                 .HasDatabaseName("UX_UserInterest_User_Name");

                e.HasOne(ui => ui.User)
                 .WithMany(u => u.UserInterests)
                 .HasForeignKey(ui => ui.UserId);

                e.HasOne(ui => ui.Interest)
                 .WithMany(i => i.UserInterests)
                 .HasForeignKey(ui => ui.InterestId);

                e.ToTable(tb =>
                {
                    tb.HasCheckConstraint("CK_UserInterest_Name_NotBlank", "LEN(LTRIM(RTRIM([Name]))) > 0");
                });
            });

            modelBuilder.ApplyConfiguration(new CountryConfig());
            modelBuilder.ApplyConfiguration(new LocationConfig());

            foreach (var fk in modelBuilder.Model.GetEntityTypes().SelectMany(et => et.GetForeignKeys()))
            {
                if (!fk.IsOwnership)
                    fk.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.Entity<TripParticipant>()
                .HasOne(tp => tp.Trip)
                .WithMany(t => t.Participants)
                .HasForeignKey(tp => tp.TripId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserInterest>()
                .HasOne(ui => ui.User)
                .WithMany(u => u.UserInterests)
                .HasForeignKey(ui => ui.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserInterest>()
                .HasOne(ui => ui.Interest)
                .WithMany(i => i.UserInterests)
                .HasForeignKey(ui => ui.InterestId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}