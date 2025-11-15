using Microsoft.EntityFrameworkCore;
using static AutoZone.Models.Enum;

namespace AutoZone.Models
{
    public class AutoZoneDbContext : DbContext

    {

        public AutoZoneDbContext(DbContextOptions<AutoZoneDbContext> options)
        : base(options)
        {
        }


        public DbSet<Car> Cars { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           

            modelBuilder.Entity<Car>()
                .HasOne(i => i.Owner)
                .WithMany(d => d.Cars)
                .HasForeignKey(i => i.UserId);

            modelBuilder.Entity<Rental>()
                .HasOne(i => i.Car)
                .WithMany(d => d.Rentals)
                .HasForeignKey(i => i.CarId)
                .OnDelete(DeleteBehavior.Cascade);



            modelBuilder.Entity<Rental>()
                .HasOne(i => i.Renter)
                .WithMany(d => d.Rentals)
                .HasForeignKey(i => i.RenterId)
                .OnDelete(DeleteBehavior.Restrict);


        }

    }
}
