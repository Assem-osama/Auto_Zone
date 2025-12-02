using AutoZone.Models;
using AutoZone.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AutoZone.Repositories
{
    public class UserRepository:GenericRepository<User>, IUserRepository
    {
        private readonly AutoZonedbContext db;

        public UserRepository(AutoZonedbContext db):base(db)
        {
            this.db = db;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await db.Users
                .Include(u=>u.Cars)
                .Include(u=>u.Rentals)
                .FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
        }
        public async Task<IEnumerable<User>> GetAllAdminsAsync()
        {
            return await db.Users.Where(i=>i.Role==Models.Enum.UserRole.Admin)
                .Include(u => u.Cars)
                .ToListAsync();
        }
    }
}
