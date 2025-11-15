using AutoZone.Models;
using AutoZone.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AutoZone.Repositories
{
    public class CarRepository: GenericRepository<Car>, ICarRepository
    {
        private readonly AutoZoneDbContext db;

        public CarRepository(AutoZoneDbContext db):base(db) 
        {
            this.db = db;
        }

        public async Task<IEnumerable<Car>> GetCarsByBrandAsync(string brand)
        {
            return await db.Cars.Where(i=>i.Brand.ToLower() == brand.ToLower())
                .Include(c => c.Owner)
                .ToListAsync();
        }

        public async Task<IEnumerable<Car>> GetCarsForSaleAsync()
        {
            return await db.Cars.Where(i=>i.status==Models.Enum.CarStatus.ForSale)
                .Include(u=>u.Owner)
                .ToListAsync();
        }

        public async Task<IEnumerable<Car>> GetCarsForRentAsync()
        {
            return await db.Cars.Where(i => i.status == Models.Enum.CarStatus.ForRent)
               .Include(u => u.Owner)
               .ToListAsync();
        }


        public async Task<IEnumerable<Car>> GetRentedCarsAsync()
        {
            return await db.Cars.Where(i => i.status == Models.Enum.CarStatus.Rented)
               .Include(u => u.Owner)
               .ToListAsync();
        }


        public async Task<IEnumerable<Car>> GetSoldCarsAsync()
        {
            return await db.Cars.Where(i => i.status == Models.Enum.CarStatus.Sold)
               .Include(u => u.Owner)
               .ToListAsync();
        }

    }
}
