using AutoZone.Models;
using AutoZone.Repositories.Interfaces;

namespace AutoZone.Repository.Interfaces
{
    public interface IRentalRepository : IGenericRepository<Rental>
    {
        Task<IEnumerable<Rental>> GetRentalsByUserIdAsync(int userId);
        Task<IEnumerable<Rental>> GetRentalsByCarIdAsync(int carId);
    }
}
