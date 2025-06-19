using CustomerSuccessCRM.Lib.Models;
using CustomerSuccessCRM.Lib.Data;
using Microsoft.EntityFrameworkCore;

namespace CustomerSuccessCRM.Lib.Repositories
{
    public class MetaRepository(CrmDbContext context) : Repository<Meta>(context), IMetaRepository
    {
        public async Task<IEnumerable<Meta>> GetMetasByResponsavelAsync(string responsavelId)
        {
            return await _dbSet
                .Include(m => m.Responsavel)
                .Where(m => m.ResponsavelId == responsavelId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Meta>> GetMetasByEquipeAsync(string equipeId)
        {
            return await _dbSet
                .Include(m => m.Equipe)
                .Where(m => equipeId.Equals(m.EquipeId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Meta>> GetMetasByPeriodoAsync(DateTime inicio, DateTime fim)
        {
            return await _dbSet
                .Include(m => m.Responsavel)
                .Where(m => m.DataInicio >= inicio && m.DataFim <= fim)
                .ToListAsync();
        }

        public async Task<IEnumerable<Meta>> GetMetasAtrasadasAsync()
        {
            var hoje = DateTime.Today;
            return await _dbSet
                .Include(m => m.Responsavel)
                .Where(m => m.Status == StatusMeta.EmAndamento && m.DataFim < hoje)
                .ToListAsync();
        }

        public async Task<IEnumerable<Meta>> GetMetasProximasVencerAsync(int dias)
        {
            var dataLimite = DateTime.Today.AddDays(dias);
            return await _dbSet
                .Include(m => m.Responsavel)
                .Where(m => m.Status == StatusMeta.EmAndamento && m.DataFim <= dataLimite)
                .ToListAsync();
        }

        public async Task<decimal> GetPercentualAtingimentoGeralAsync(string? equipeId = null)
        {
            var query = _dbSet.AsQueryable();
            if (!string.IsNullOrEmpty(equipeId))
            {
                query = query.Where(m => equipeId.Equals(m.EquipeId));
            }

            var metas = await query.ToListAsync();
            if (!metas.Any()) return 0;

            var totalMetas = metas.Count;
            var metasAtingidas = metas.Count(m => m.Status == StatusMeta.Concluida);

            return (decimal)metasAtingidas / totalMetas * 100;
        }

        public async Task<IEnumerable<MetaHistorico>> GetHistoricoMetaAsync(int metaId)
        {
            return await _context.MetaHistoricos
                .Where(h => metaId == h.MetaId)
                .OrderByDescending(h => h.Id)
                .ToListAsync();
        }

        public async Task<IDictionary<TipoMeta, decimal>> GetAtingimentoPorTipoAsync()
        {
            var metas = await _dbSet.ToListAsync();
            return metas.GroupBy(m => m.Tipo)
                .ToDictionary(
                    g => g.Key,
                    g => g.Count(m => m.Status == StatusMeta.Concluida) / (decimal)g.Count() * 100
                );
        }

        public async Task<IDictionary<string, decimal>> GetAtingimentoPorEquipeAsync()
        {
            var metas = await _dbSet
                .Include(m => m.Equipe)
                .ToListAsync();

            return metas.GroupBy(m => m.EquipeId)
                .ToDictionary<IGrouping<int?, Meta>, string, decimal>(
                    g => g.Key.ToString() ?? string.Empty,
                    g => g.Count(m => m.Status == StatusMeta.Concluida) / (decimal)g.Count() * 100
                );
        }

        public async Task<IEnumerable<Meta>> GetMetasRecorrentesAsync()
        {
            return await _dbSet
                .Include(m => m.Responsavel)
                .Where(m => m.Recorrente)
                .ToListAsync();
        }
    }
} 