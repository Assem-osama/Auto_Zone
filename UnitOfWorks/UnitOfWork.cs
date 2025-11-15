using AutoZone.Models;
using AutoZone.Repositories;
using AutoZone.Repositories.Interfaces;
using AutoZone.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AutoZone.UnitOfWorks
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly AutoZoneDbContext db;

        public UnitOfWork(AutoZoneDbContext db)
        {
            this.db = db;
        }

        private ICarRepository _carRepo;
        private IUserRepository _userRepo;
        private IRentalRepository _rentalRepo;

        public ICarRepository Cars
        {
            get { return _carRepo ??= new CarRepository(db); }
        }
        public IUserRepository Users
        {
            get
            {
                return _userRepo ??= new UserRepository(db);
            }
        }

        public IRentalRepository Rentals
        {
            get
            {
                return _rentalRepo ??= new RentalRepository(db);
            }
        }

        public async Task<int> SaveAsync()
        {
            return await db.SaveChangesAsync();
        }

        public void Dispose()
        {
            db.Dispose();
        }



    }
}
