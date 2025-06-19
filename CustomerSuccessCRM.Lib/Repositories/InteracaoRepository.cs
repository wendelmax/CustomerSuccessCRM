using Microsoft.EntityFrameworkCore;
using CustomerSuccessCRM.Lib.Data;
using CustomerSuccessCRM.Lib.Models;

namespace CustomerSuccessCRM.Lib.Repositories
{
    public class InteracaoRepository : Repository<Interacao>, IInteracaoRepository
    {
        public InteracaoRepository(CrmDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Interacao>> GetAllAsync()
        {
            return await _dbSet
                .Include(i => i.Cliente)
                .OrderByDescending(i => i.DataInteracao)
                .ToListAsync();
        }

        public override async Task<Interacao?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(i => i.Cliente)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<Interacao>> GetByClienteIdAsync(int clienteId)
        {
            return await _dbSet
                .Where(i => i.ClienteId == clienteId)
                .OrderByDescending(i => i.DataInteracao)
                .ToListAsync();
        }

        public async Task<IEnumerable<Interacao>> GetByTipoAsync(TipoInteracao tipo)
        {
            return await _dbSet
                .Include(i => i.Cliente)
                .Where(i => i.Tipo == tipo)
                .OrderByDescending(i => i.DataInteracao)
                .ToListAsync();
        }

        public async Task<IEnumerable<Interacao>> GetByStatusAsync(StatusInteracao status)
        {
            return await _dbSet
                .Include(i => i.Cliente)
                .Where(i => i.Status == status)
                .OrderByDescending(i => i.DataInteracao)
                .ToListAsync();
        }

        public async Task<IEnumerable<Interacao>> GetByResponsavelAsync(string responsavel)
        {
            return await _dbSet
                .Include(i => i.Cliente)
                .Where(i => i.Responsavel != null && i.Responsavel.Contains(responsavel))
                .OrderByDescending(i => i.DataInteracao)
                .ToListAsync();
        }

        public async Task<IEnumerable<Interacao>> GetByDataRangeAsync(DateTime dataInicio, DateTime dataFim)
        {
            return await _dbSet
                .Include(i => i.Cliente)
                .Where(i => i.DataInteracao >= dataInicio && i.DataInteracao <= dataFim)
                .OrderByDescending(i => i.DataInteracao)
                .ToListAsync();
        }

        public async Task<IEnumerable<Interacao>> GetInteracoesPendentesAsync()
        {
            return await _dbSet
                .Include(i => i.Cliente)
                .Where(i => i.Status == StatusInteracao.Aberta || i.Status == StatusInteracao.EmAndamento)
                .OrderByDescending(i => i.DataInteracao)
                .ToListAsync();
        }

        public async Task<IEnumerable<Interacao>> GetInteracoesUrgentesAsync()
        {
            return await _dbSet
                .Include(i => i.Cliente)
                .Where(i => i.Prioridade == PrioridadeInteracao.Urgente && 
                           (i.Status == StatusInteracao.Aberta || i.Status == StatusInteracao.EmAndamento))
                .OrderByDescending(i => i.DataInteracao)
                .ToListAsync();
        }

        public async Task<int> GetTotalInteracoesAsync(int clienteId)
        {
            return await _dbSet
                .Where(i => i.ClienteId == clienteId)
                .CountAsync();
        }
    }
} 