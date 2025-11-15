using AutoZone.Repositories.Interfaces;
using AutoZone.Repository.Interfaces;

namespace AutoZone.UnitOfWorks
{
    public interface IUnitOfWork:IDisposable
    {
        ICarRepository Cars { get; }
        IUserRepository Users { get; }
        IRentalRepository Rentals { get; }

        Task<int> SaveAsync();

    }
}
