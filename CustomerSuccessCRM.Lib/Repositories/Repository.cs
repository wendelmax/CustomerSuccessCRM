using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using CustomerSuccessCRM.Lib.Data;

namespace CustomerSuccessCRM.Lib.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<T?> GetByIdWithIncludesAsync(int id, params Expression<Func<T, object>>[] includes)
        {
            var query = _dbSet.AsQueryable();
            
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> GetAllWithIncludesAsync(params Expression<Func<T, object>>[]? includes)
        {
            var query = _dbSet.AsQueryable();
            
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            return await query.ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> FindWithIncludesAsync(
            Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[]? includes)
        {
            var query = _dbSet.Where(predicate);
            
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            return await query.ToListAsync();
        }

        public virtual async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public virtual async Task<T?> FirstOrDefaultWithIncludesAsync(
            Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[]? includes)
        {
            var query = _dbSet.AsQueryable();
            
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            return await query.FirstOrDefaultAsync(predicate);
        }

        public virtual async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
        {
            if (predicate == null)
                return await _dbSet.CountAsync();
            
            return await _dbSet.CountAsync(predicate);
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            var entityList = entities.ToList();
            await _dbSet.AddRangeAsync(entityList);
            await _context.SaveChangesAsync();
            return entityList;
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task UpdateRangeAsync(IEnumerable<T> entities)
        {
            _dbSet.UpdateRange(entities);
            await _context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public virtual async Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
            await _context.SaveChangesAsync();
        }

        public virtual async Task<bool> ExistsAsync(int id)
        {
            return await GetByIdAsync(id) != null;
        }

        public virtual IQueryable<T> GetQueryable()
        {
            return _dbSet.AsQueryable();
        }

        public virtual async Task<IEnumerable<T>> GetPagedAsync(
            int page,
            int pageSize,
            Expression<Func<T, object>> orderBy,
            bool ascending = true,
            Expression<Func<T, bool>>? filter = null,
            params Expression<Func<T, object>>[]? includes)
        {
            var query = _dbSet.AsQueryable();

            if (filter != null)
                query = query.Where(filter);

            if (includes != null)
                query = includes.Aggregate(query, (current, include) => current.Include(include));

            query = ascending
                ? query.OrderBy(orderBy)
                : query.OrderByDescending(orderBy);

            return await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public virtual async Task<(IEnumerable<T> Items, int TotalCount)> GetPagedWithTotalAsync(
            int page,
            int pageSize,
            Expression<Func<T, object>> orderBy,
            bool ascending = true,
            Expression<Func<T, bool>>? filter = null,
            params Expression<Func<T, object>>[]? includes)
        {
            var query = _dbSet.AsQueryable();

            if (filter != null)
                query = query.Where(filter);

            var totalCount = await query.CountAsync();

            if (includes != null)
                query = includes.Aggregate(query, (current, include) => current.Include(include));

            query = ascending
                ? query.OrderBy(orderBy)
                : query.OrderByDescending(orderBy);

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public virtual void DetachEntity(T entity)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }

        public virtual void DetachAllEntities()
        {
            var changedEntriesCopy = _context.ChangeTracker.Entries()
                .Where(e => e.State != EntityState.Detached)
                .ToList();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;
        }
    }
} 