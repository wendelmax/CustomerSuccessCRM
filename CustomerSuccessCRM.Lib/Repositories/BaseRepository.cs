using CustomerSuccessCRM.Lib.Data;
using Microsoft.EntityFrameworkCore;

namespace CustomerSuccessCRM.Lib.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly CrmDbContext _context;
        protected readonly DbSet<T> _dbSet;

        protected BaseRepository(CrmDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public virtual async Task<bool> ExisteAsync(int id)
        {
            return await _dbSet.FindAsync(id) != null;
        }

        public virtual async Task<bool> AdicionarAsync(T entidade)
        {
            await _dbSet.AddAsync(entidade);
            return await SaveChangesAsync();
        }

        public virtual async Task<bool> AtualizarAsync(T entidade)
        {
            _dbSet.Update(entidade);
            return await SaveChangesAsync();
        }

        public virtual async Task<bool> DeletarAsync(int id)
        {
            var entidade = await _dbSet.FindAsync(id);
            if (entidade == null) return false;

            _dbSet.Remove(entidade);
            return await SaveChangesAsync();
        }
    }
} 