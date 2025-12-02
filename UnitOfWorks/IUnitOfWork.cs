using AutoZone.Repositories.Interfaces;
using AutoZone.Repository.Interfaces;

namespace AutoZone.UnitOfWorks
{
    public interface IUnitOfWork
    {
        ICarRepository Cars { get; }
        IUserRepository Users { get; }
        IRentalRepository Rentals { get; }

        IPaymentTransactionRepository PaymentTransactions { get; }

        Task<int> SaveAsync();

    }
}
