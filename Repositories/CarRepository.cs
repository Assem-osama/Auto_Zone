using AutoZone.DTOs;
using AutoZone.DTOs.Car;
using AutoZone.Models;
using AutoZone.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AutoZone.Repositories
{
    public class CarRepository : GenericRepository<Car>, ICarRepository
    {
        private readonly AutoZonedbContext _context;

        public CarRepository(AutoZonedbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PagedResult<Car>> GetCarsAsync(CarQueryParameters parameters)
        {
            var query = _context.Cars.AsQueryable();

            // 1. Filtering
            if (!string.IsNullOrWhiteSpace(parameters.Brand))
            {
                query = query.Where(c => c.Brand.Contains(parameters.Brand));
            }

            if (parameters.MinPrice.HasValue)
            {
                query = query.Where(c => c.Price >= parameters.MinPrice.Value);
            }

            if (parameters.MaxPrice.HasValue)
            {
                query = query.Where(c => c.Price <= parameters.MaxPrice.Value);
            }

            // 2. Sorting
            if (!string.IsNullOrWhiteSpace(parameters.SortBy))
            {
                if (parameters.SortDescending)
                {
                    query = parameters.SortBy.ToLower() switch
                    {
                        "price" => query.OrderByDescending(c => c.Price),
                        "year" => query.OrderByDescending(c => c.Year),
                        "brand" => query.OrderByDescending(c => c.Brand),
                        _ => query.OrderByDescending(c => c.Id)
                    };
                }
                else
                {
                    query = parameters.SortBy.ToLower() switch
                    {
                        "price" => query.OrderBy(c => c.Price),
                        "year" => query.OrderBy(c => c.Year),
                        "brand" => query.OrderBy(c => c.Brand),
                        _ => query.OrderBy(c => c.Id)
                    };
                }
            }
            else
            {
                // Default sort
                query = query.OrderByDescending(c => c.Id);
            }

            // 3. Pagination
            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            return new PagedResult<Car>(items, totalCount, parameters.PageNumber, parameters.PageSize);
        }

        public async Task<IEnumerable<Car>> GetCarsByBrandAsync(string brand)
        {
            return await _context.Cars.Where(i => i.Brand.ToLower() == brand.ToLower())
                .Include(c => c.Owner)
                .ToListAsync();
        }

        public async Task<IEnumerable<Car>> GetCarsForSaleAsync()
        {
            return await _context.Cars.Where(i => i.status == Models.Enum.CarStatus.ForSale)
                .Include(u => u.Owner)
                .ToListAsync();
        }

        public async Task<IEnumerable<Car>> GetCarsForRentAsync()
        {
            return await _context.Cars.Where(i => i.status == Models.Enum.CarStatus.ForRent)
               .Include(u => u.Owner)
               .ToListAsync();
        }

        public async Task<IEnumerable<Car>> GetRentedCarsAsync()
        {
            return await _context.Cars.Where(i => i.status == Models.Enum.CarStatus.Rented)
               .Include(u => u.Owner)
               .ToListAsync();
        }


        public async Task<IEnumerable<Car>> GetSoldCarsAsync()
        {
            return await _context.Cars.Where(i => i.status == Models.Enum.CarStatus.Sold)
               .Include(u => u.Owner)
               .ToListAsync();
        }
    }
}
