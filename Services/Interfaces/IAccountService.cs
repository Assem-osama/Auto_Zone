using System.Threading.Tasks;
using AutoZone.DTOs;
using AutoZone.Models;

namespace AutoZone.Services.Interfaces
{
    public interface IAccountService
    {
        Task<ServiceResponse<string>> RegisterAsync(RegisterDTO registerDto);
        Task<ServiceResponse<string>> LoginAsync(LoginDTO loginDto);
    }
}
