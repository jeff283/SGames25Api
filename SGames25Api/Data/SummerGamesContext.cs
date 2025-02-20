using System;
using Microsoft.EntityFrameworkCore;
using SGames25Api.Models;
using SGames25Api.Models.Audit;

namespace SGames25Api.Data
{
    public class SummerGamesContext : DbContext
    {
        //To give access to IHttpContextAccessor for Audit Data with IAuditable
        private readonly IHttpContextAccessor _httpContextAccessor;

        //Property to hold the UserName value
        public string UserName
        {
            get; private set;
        }

        public SummerGamesContext(DbContextOptions<SummerGamesContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            if (_httpContextAccessor.HttpContext != null)
            {
                //We have a HttpContext, but there might not be anyone Authenticated
                UserName = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Unknown";
            }
            else
            {
                //No HttpContext so seeding data
                UserName = "Seed Data";
            }
        }

        public SummerGamesContext(DbContextOptions<SummerGamesContext> options)
       : base(options)
        {
            _httpContextAccessor = null!;
            UserName = "Seed Data";
        }

        public DbSet<Athlete> Athletes { get; set; }
        public DbSet<Contingent> Contingents { get; set; }
        public DbSet<Sport> Sports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Add unique constraints
            modelBuilder.Entity<Athlete>()
                .HasIndex(a => a.AthleteCode)
                .IsUnique();

            modelBuilder.Entity<Contingent>()
                .HasIndex(c => c.Code)
                .IsUnique();

            modelBuilder.Entity<Sport>()
                .HasIndex(s => s.Code)
                .IsUnique();

            // Configure relationships with Cascade Delete restricted
            modelBuilder.Entity<Contingent>()
                .HasMany(c => c.Athletes)
                .WithOne(a => a.Contingent)
                .HasForeignKey(a => a.ContingentID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Sport>()
                .HasMany(s => s.Athletes)
                .WithOne(a => a.Sport)
                .HasForeignKey(a => a.SportID)
                .OnDelete(DeleteBehavior.Restrict);

        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void OnBeforeSaving()
        {
            var entries = ChangeTracker.Entries();
            foreach (var entry in entries)
            {
                if (entry.Entity is IAuditable trackable)
                {
                    var now = DateTime.UtcNow;
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            trackable.UpdatedOn = now;
                            trackable.UpdatedBy = UserName;
                            break;

                        case EntityState.Added:
                            trackable.CreatedOn = now;
                            trackable.CreatedBy = UserName;
                            trackable.UpdatedOn = now;
                            trackable.UpdatedBy = UserName;
                            break;
                    }
                }
            }
        }
    }
}
