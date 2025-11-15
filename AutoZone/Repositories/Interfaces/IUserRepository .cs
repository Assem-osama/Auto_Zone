using AutoZone.Models;
using AutoZone.Repositories.Interfaces;

namespace AutoZone.Repository.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllAdminsAsync();
    }
}
