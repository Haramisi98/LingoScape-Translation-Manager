using LingoScape.DataAccessLayer;
using LingoScape.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LingoScape.Repository.Implementations
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected readonly LingoDbContext _context;
        protected readonly DbSet<T> _dbSet;

        protected RepositoryBase(LingoDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public virtual IEnumerable<T> ReadAll() => _dbSet;

        public virtual T Read(int id) => _dbSet.Find(id);

        public virtual T Create(T entity)
        {
            var result = _dbSet.Add(entity);
            _context.SaveChanges();
            return result.Entity;
        }

        public virtual T Update(T entity)
        {
            var result = _dbSet.Update(entity);
            _context.SaveChanges();
            return result.Entity;
        }

        public virtual void Delete(int id)
        {
            var entity = Read(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                _context.SaveChanges();
            }
            else
            {
                throw new KeyNotFoundException($"Entity of type {typeof(T).Name} with ID {id} not found.");
            }
        }
    }
}
