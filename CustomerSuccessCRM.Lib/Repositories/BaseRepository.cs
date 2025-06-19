using CustomerSuccessCRM.Lib.Data;
using Microsoft.EntityFrameworkCore;

namespace CustomerSuccessCRM.Lib.Repositories
{
    public abstract class BaseRepository : IBaseRepository
    {
        protected readonly CrmDbContext _context;

        protected BaseRepository(CrmDbContext context, string nomeEntidade)
        {
            _context = context;
        }

        public virtual async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public virtual async Task<bool> ExisteAsync(int id)
        {
            // Implementação específica deve ser feita nas classes filhas
            return false;
        }

        public virtual async Task<bool> AdicionarAsync(object entidade)
        {
            // Implementação específica deve ser feita nas classes filhas
            return false;
        }

        public virtual async Task<bool> AtualizarAsync(object entidade)
        {
            // Implementação específica deve ser feita nas classes filhas
            return false;
        }

        public virtual async Task<bool> DeletarAsync(int id)
        {
            // Implementação específica deve ser feita nas classes filhas
            return false;
        }
    }
} 