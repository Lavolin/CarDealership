using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarDealership.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<Car> Cars { get; set; } = null!;

        public DbSet<CarCategory> CarCategories { get; set; } = null!;

        public DbSet<Dealer> Dealers { get; set; } = null!;

        public DbSet<Motor> Motors { get; set; } = null!;

        public DbSet<MotorCategory> MotorCategories { get; set; } = null!;

        public DbSet<Truck> Trucks { get; set; } = null!;

        public DbSet<TruckCategory> TruckCategories { get; set; } = null!;
    }
}