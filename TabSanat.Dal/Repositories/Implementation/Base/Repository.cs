using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TabSanat.Dal.Repositories.Interfaces.Base;

namespace TabSanat.Dal.Repositories.Implementation.Base
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected DbContext _context;
        private DbSet<TEntity> _dbSet;
        public Repository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }
        public virtual void Add(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public void AddRange(IQueryable<TEntity> entities)
        {
            _dbSet.AddRange(entities);
        }

        public IQueryable<TEntity> GetAll()
        {
            return _dbSet.AsQueryable();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<IQueryable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> result = _dbSet;
            if (filter != null)
                result = result.Where(filter);
            if (orderBy != null)
                result = orderBy(result);
            foreach (var include in includes)
                result = result.Include(include);

            await result.ToListAsync();
            return result;
        }

        public TEntity GetById(Guid id)
        {
            return _dbSet.Find(id);
        }

        public TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> result = _dbSet;
            foreach (var include in includes)
            {
                result = result.Include(include);
            }
            return result.FirstOrDefault(predicate);
        }
        public async Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> result = _dbSet;
            foreach (var include in includes)
            {
                result = result.Include(include);
            }
            return await result.FirstOrDefaultAsync(predicate);
        }

        public IQueryable<TEntity> GetMany(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> result = _dbSet;
            if (filter != null)
                result = result.Where(filter);
            if (orderBy != null)
                result = orderBy(result);
            foreach (var include in includes)
                result = result.Include(include);

            return result;
        }

        public void Remove(Guid id)
        {
            _dbSet.Remove(GetById(id));
        }

        public void RemoveEntity(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public void RemoveRange(IQueryable<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
        }
        public void Update(TEntity entity)
        {
            _dbSet.Attach(entity).State = EntityState.Modified;
        }

    }
}

