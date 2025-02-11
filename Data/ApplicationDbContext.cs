using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Reflection.Emit;

namespace HMS.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Log_21180040> Log_21180040 { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Customer>()
                .HasIndex(c => c.PhoneNumber)
                .IsUnique();

            builder.Entity<Reservation>()
                .Property(r => r.TotalPrice)
                .HasPrecision(18, 2);

            builder.Entity<Room>()
                .Property(r => r.PricePerNight)
                .HasPrecision(18, 2);

        }

        public override int SaveChanges()
        {
            var logEntries = new List<Log_21180040>();

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.State == EntityState.Deleted)
                {
                    logEntries.Add(new Log_21180040
                    {
                        TableName = entry.Metadata.GetTableName(),
                        TypeOfOperation = entry.State.ToString(),
                        OperationTime = DateTime.UtcNow
                    });
                }
            }

            // Save actual entity changes first
            var result = base.SaveChanges();

            // Insert logs separately to avoid modifying the tracked entities during iteration
            if (logEntries.Any())
            {
                Log_21180040.AddRange(logEntries);
                base.SaveChanges();
            }

            return result;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var logEntries = new List<Log_21180040>();

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.State == EntityState.Deleted)
                {
                    logEntries.Add(new Log_21180040
                    {
                        TableName = entry.Metadata.GetTableName(),
                        TypeOfOperation = entry.State.ToString(),
                        OperationTime = DateTime.UtcNow
                    });
                }
            }

            // Save actual entity changes first
            var result = await base.SaveChangesAsync(cancellationToken);

            // Insert logs separately to avoid modifying the tracked entities during iteration
            if (logEntries.Any())
            {
                Log_21180040.AddRange(logEntries);
                await base.SaveChangesAsync(cancellationToken);
            }

            return result;
        }

    }

}
