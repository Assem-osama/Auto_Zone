using AutoZone.Models;
using AutoZone.Repositories;
using AutoZone.Repositories.Interfaces;
using AutoZone.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.Options;

namespace AutoZone.UnitOfWorks
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly AutoZonedbContext db;
        private readonly IOptions<StripeSettings> _stripeSettings;

        public UnitOfWork(AutoZonedbContext db, IOptions<StripeSettings> stripeSettings)
        {
            this.db = db;
            _stripeSettings = stripeSettings;
        }

        private ICarRepository _carRepo;
        private IUserRepository _userRepo;
        private IRentalRepository _rentalRepo;
        private IPaymentTransactionRepository _paymentTransactionRepo;

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

        public IPaymentTransactionRepository PaymentTransactions
        {
            get
            {
                return _paymentTransactionRepo ??= new PaymentTransactionRepository(db, _stripeSettings);
            }
        }
        public async Task<int> SaveAsync()
        {
            return await db.SaveChangesAsync();
        }

     



    }
}
