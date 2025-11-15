using AutoZone.DTOs;
using AutoZone.DTOs.Rental;
using AutoZone.Models;

namespace AutoZone.Services.Interfaces
{
    public interface IRentalService
    {
        Task<ServiceResponse<IEnumerable<RentalDTO>>> GetAllRentalsAsync();
        Task<ServiceResponse<RentalDTO>> GetRentalByIdAsync(int id);
        Task<ServiceResponse<RentalDTO>> CreateRentalAsync(CreateRentalDTO dto, int userId);
        Task<ServiceResponse<string>> UpdateRentalAsync(int id, UpdateRentalDTO dto, int userId);
        Task<ServiceResponse<string>> DeleteRentalAsync(int id, int userId);

    }
}
