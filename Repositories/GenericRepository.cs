using System.Linq;
using System.Linq.Expressions;
using AutoZone.Models;
using AutoZone.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AutoZone.Repositories
{
    public class GenericRepository<TEntity>:IGenericRepository<TEntity> where TEntity : class
    {
        private readonly AutoZonedbContext db;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(AutoZonedbContext db)
        {
            this.db = db;
            _dbSet = db.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            //return db.Set<TEntity>().ToList();
            return  await _dbSet.ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<TEntity?> GetByNameAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);

            //await _userRepository.GetByNameAsync(u => u.Name == "Assem");
            //بنستعملها كدا يعني بنحط بين القوسين الشرط اللي عايزينو يحصل

        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();

            //await _carRepository.FindAsync(c => c.Price > 500000);

        }


        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }


    }
}
