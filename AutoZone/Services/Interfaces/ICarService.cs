using AutoZone.DTOs;
using AutoZone.DTOs.Car;
using AutoZone.Models;

namespace AutoZone.Services.Interfaces
{
    public interface ICarService
    {
        Task<ServiceResponse<IEnumerable<CarDTO>>> GetAllCarsAsync();
        Task<ServiceResponse<CarDTO>> GetCarByIdAsync(int id);
        Task<ServiceResponse<CarDTO>> CreateCarAsync(CreateCarDTO dto, int userId);
        Task<ServiceResponse<string>> UpdateCarAsync(int id, UpdateCarDTO dto);
        Task<ServiceResponse<string>> DeleteCarAsync(int id);
    }
}
