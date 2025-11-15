using AutoZone.Models;
using AutoZone.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AutoZone.Repositories
{
    public class RentalRepository : GenericRepository<Rental>, IRentalRepository
    {
        private readonly AutoZoneDbContext db;

        public RentalRepository(AutoZoneDbContext db) : base(db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<Rental>> GetRentalsByCarIdAsync(int carId)
        {
            return await db.Rentals.Where(i=>i.CarId == carId)
                .Include(r => r.Renter)
                .ToListAsync();
        }

        public async Task<IEnumerable<Rental>> GetRentalsByUserIdAsync(int userId)
        {
            return await db.Rentals.Where(r => r.RenterId == userId)
                .Include(r => r.Car)
                .ThenInclude(c => c.Owner)
                .ToListAsync();
        }

    }
}
