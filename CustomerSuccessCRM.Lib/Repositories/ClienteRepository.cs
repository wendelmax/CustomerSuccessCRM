using CustomerSuccessCRM.Lib.Models;
using CustomerSuccessCRM.Lib.Data;
using Microsoft.EntityFrameworkCore;

namespace CustomerSuccessCRM.Lib.Repositories
{
    public class ClienteRepository : BaseRepository<Cliente>, IClienteRepository
    {
        public ClienteRepository(CrmDbContext context) : base(context)
        {
        }

        public async Task<List<Cliente>> BuscarTodosAsync()
        {
            return await _dbSet
                .Where(c => c.Ativo)
                .ToListAsync();
        }

        public async Task<Cliente?> BuscarPorIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<List<Cliente>> BuscarPorStatusAsync(StatusCliente status)
        {
            return await _dbSet
                .Where(c => c.Status == status && c.Ativo)
                .ToListAsync();
        }

        public async Task<List<Cliente>> BuscarPorVendedorAsync(string vendedorId)
        {
            return await _dbSet
                .Where(c => c.VendedorId == vendedorId && c.Ativo)
                .ToListAsync();
        }

        public async Task<List<Cliente>> BuscarInativos()
        {
            return await _dbSet
                .Where(c => !c.Ativo)
                .ToListAsync();
        }

        public async Task<int> ContarClientesAtivosAsync()
        {
            return await _dbSet
                .CountAsync(c => c.Ativo);
        }
    }
}