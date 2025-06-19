using System.Linq.Expressions;

namespace CustomerSuccessCRM.Lib.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<T?> GetByIdWithIncludesAsync(int id, params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllWithIncludesAsync(params Expression<Func<T, object>>[]? includes);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> FindWithIncludesAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[]? includes);
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task<T?> FirstOrDefaultWithIncludesAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[]? includes);
        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        Task<T> AddAsync(T entity);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);
        Task<T> UpdateAsync(T entity);
        Task UpdateRangeAsync(IEnumerable<T> entities);
        Task DeleteAsync(int id);
        Task DeleteRangeAsync(IEnumerable<T> entities);
        Task<bool> ExistsAsync(int id);
        IQueryable<T> GetQueryable();
        Task<IEnumerable<T>> GetPagedAsync(
            int page,
            int pageSize,
            Expression<Func<T, object>> orderBy,
            bool ascending = true,
            Expression<Func<T, bool>>? filter = null,
            params Expression<Func<T, object>>[]? includes);
        Task<(IEnumerable<T> Items, int TotalCount)> GetPagedWithTotalAsync(
            int page,
            int pageSize,
            Expression<Func<T, object>> orderBy,
            bool ascending = true,
            Expression<Func<T, bool>>? filter = null,
            params Expression<Func<T, object>>[]? includes);
        void DetachEntity(T entity);
        void DetachAllEntities();
    }
} 