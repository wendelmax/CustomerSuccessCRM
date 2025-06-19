using Microsoft.EntityFrameworkCore;
using CustomerSuccessCRM.Lib.Data;
using CustomerSuccessCRM.Lib.Models;

namespace CustomerSuccessCRM.Lib.Repositories
{
    public class ClienteRepository : Repository<Cliente>, IClienteRepository
    {
        public ClienteRepository(CrmDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Cliente>> GetAllAsync()
        {
            return await _dbSet
                .Include(c => c.Interacoes)
                .Include(c => c.Oportunidades)
                .OrderBy(c => c.Nome)
                .ToListAsync();
        }

        public override async Task<Cliente?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(c => c.Interacoes.OrderByDescending(i => i.DataInteracao))
                .Include(c => c.Oportunidades.OrderByDescending(o => o.DataCriacao))
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Cliente>> GetByStatusAsync(StatusCliente status)
        {
            return await _dbSet
                .Where(c => c.Status == status)
                .OrderBy(c => c.Nome)
                .ToListAsync();
        }

        public async Task<IEnumerable<Cliente>> GetByEmpresaAsync(string empresa)
        {
            return await _dbSet
                .Where(c => c.Empresa != null && c.Empresa.Contains(empresa))
                .OrderBy(c => c.Nome)
                .ToListAsync();
        }

        public async Task<IEnumerable<Cliente>> GetByCidadeAsync(string cidade)
        {
            return await _dbSet
                .Where(c => c.Cidade != null && c.Cidade.Contains(cidade))
                .OrderBy(c => c.Nome)
                .ToListAsync();
        }

        public async Task<Cliente?> GetByEmailAsync(string email)
        {
            return await _dbSet
                .FirstOrDefaultAsync(c => c.Email == email);
        }

        public async Task<IEnumerable<Cliente>> SearchAsync(string searchTerm)
        {
            var term = searchTerm.ToLower();
            return await _dbSet
                .Where(c => c.Nome.ToLower().Contains(term) ||
                           c.Sobrenome.ToLower().Contains(term) ||
                           c.Email.ToLower().Contains(term) ||
                           (c.Empresa != null && c.Empresa.ToLower().Contains(term)) ||
                           (c.Telefone != null && c.Telefone.Contains(term)))
                .OrderBy(c => c.Nome)
                .ToListAsync();
        }

        public async Task<IEnumerable<Cliente>> GetClientesVipAsync()
        {
            return await _dbSet
                .Where(c => c.Status == StatusCliente.ClienteVip)
                .OrderBy(c => c.Nome)
                .ToListAsync();
        }

        public async Task<IEnumerable<Cliente>> GetProspectosAsync()
        {
            return await _dbSet
                .Where(c => c.Status == StatusCliente.Prospecto)
                .OrderBy(c => c.Nome)
                .ToListAsync();
        }

        public async Task<int> GetTotalClientesAsync()
        {
            return await _dbSet.CountAsync();
        }

        public async Task<decimal> GetValorTotalOportunidadesAsync(int clienteId)
        {
            var oportunidades = await _context.Oportunidades
                .Where(o => o.ClienteId == clienteId && o.Fase == FaseOportunidade.Fechada)
                .ToListAsync();
            return oportunidades.Sum(o => o.ValorReal);
        }
    }
} 