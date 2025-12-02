using AutoZone.DTOs;
using AutoZone.Models;

namespace AutoZone.Repositories.Interfaces
{
    public interface ICarRepository:IGenericRepository<Car>
    {
        Task<IEnumerable<Car>> GetCarsByBrandAsync(string brand);
        Task<IEnumerable<Car>> GetCarsForSaleAsync();
        Task<IEnumerable<Car>> GetCarsForRentAsync();
        Task<IEnumerable<Car>> GetSoldCarsAsync();
        Task<IEnumerable<Car>> GetRentedCarsAsync();
        
        Task<PagedResult<Car>> GetCarsAsync(AutoZone.DTOs.Car.CarQueryParameters parameters);
    }
}
