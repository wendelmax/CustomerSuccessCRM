using CustomerSuccessCRM.Lib.Models;
using CustomerSuccessCRM.Lib.Data;
using Microsoft.EntityFrameworkCore;

namespace CustomerSuccessCRM.Lib.Repositories
{
    public class ClienteRepository : BaseRepository, IClienteRepository
    {
        private readonly DbSet<Cliente> _clientes;

        public ClienteRepository(CrmDbContext context) : base(context, "Clientes")
        {
            _clientes = context.Set<Cliente>();
        }

        public override async Task<bool> ExisteAsync(int id)
        {
            return await _clientes.FindAsync(id) != null;
        }

        public override async Task<bool> AdicionarAsync(object entidade)
        {
            if (entidade is Cliente cliente)
            {
                await _clientes.AddAsync(cliente);
                return await SaveChangesAsync();
            }
            return false;
        }

        public override async Task<bool> AtualizarAsync(object entidade)
        {
            if (entidade is Cliente cliente)
            {
                _clientes.Update(cliente);
                return await SaveChangesAsync();
            }
            return false;
        }

        public override async Task<bool> DeletarAsync(int id)
        {
            var cliente = await _clientes.FindAsync(id);
            if (cliente == null) return false;

            _clientes.Remove(cliente);
            return await SaveChangesAsync();
        }

        public async Task<List<Cliente>> BuscarTodosAsync()
        {
            return await _clientes
                .Where(c => c.Ativo)
                .ToListAsync();
        }

        public async Task<Cliente> BuscarPorIdAsync(int id)
        {
            return await _clientes.FindAsync(id);
        }

        public async Task<List<Cliente>> BuscarPorStatusAsync(StatusCliente status)
        {
            return await _clientes
                .Where(c => c.Status == status && c.Ativo)
                .ToListAsync();
        }

        public async Task<List<Cliente>> BuscarPorVendedorAsync(string vendedorId)
        {
            return await _clientes
                .Where(c => c.VendedorId == vendedorId && c.Ativo)
                .ToListAsync();
        }

        public async Task<List<Cliente>> BuscarInativos()
        {
            return await _clientes
                .Where(c => !c.Ativo)
                .ToListAsync();
        }

        public async Task<int> ContarClientesAtivosAsync()
        {
            return await _clientes
                .CountAsync(c => c.Ativo);
        }
    }
} 