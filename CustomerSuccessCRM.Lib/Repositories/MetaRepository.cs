using CustomerSuccessCRM.Lib.Models;
using CustomerSuccessCRM.Lib.Data;
using Microsoft.EntityFrameworkCore;

namespace CustomerSuccessCRM.Lib.Repositories
{
    public class MetaRepository : BaseRepository<Meta>, IMetaRepository
    {
        public MetaRepository(CrmDbContext context) : base(context)
        {
        }

        public async Task<List<Meta>> BuscarTodasAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<Meta?> BuscarPorIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<List<Meta>> BuscarPorResponsavelAsync(string responsavelId)
        {
            return await _dbSet
                .Where(m => m.ResponsavelId == responsavelId)
                .ToListAsync();
        }

        public async Task<List<Meta>> BuscarPorEquipeAsync(string equipeId)
        {
            return await _dbSet
                .Where(m => m.EquipeId == equipeId)
                .ToListAsync();
        }

        public async Task<List<Meta>> BuscarAtrasadasAsync()
        {
            var hoje = DateTime.Today;
            return await _dbSet
                .Where(m => m.Status == StatusMeta.EmAndamento && m.DataFim < hoje)
                .ToListAsync();
        }

        public async Task<decimal> CalcularPercentualAtingimentoAsync(string? equipeId = null)
        {
            var query = _dbSet.AsQueryable();
            if (!string.IsNullOrEmpty(equipeId))
            {
                query = query.Where(m => m.EquipeId == equipeId);
            }

            var metas = await query.ToListAsync();
            if (!metas.Any()) return 0;

            var totalMetas = metas.Count;
            var metasAtingidas = metas.Count(m => m.Status == StatusMeta.Concluida);

            return (decimal)metasAtingidas / totalMetas * 100;
        }

        public async Task<List<Meta>> BuscarPorPeriodoAsync(DateTime inicio, DateTime fim)
        {
            return await _dbSet
                .Where(m => m.DataInicio >= inicio && m.DataFim <= fim)
                .ToListAsync();
        }

        public async Task<List<Meta>> BuscarProximasVencerAsync(int dias)
        {
            var dataLimite = DateTime.Today.AddDays(dias);
            return await _dbSet
                .Where(m => m.Status == StatusMeta.EmAndamento && m.DataFim <= dataLimite)
                .ToListAsync();
        }

        public async Task<Dictionary<string, decimal>> CalcularAtingimentoPorEquipeAsync()
        {
            var metas = await _dbSet.ToListAsync();

            return metas.GroupBy(m => m.EquipeId ?? "Sem Equipe")
                .ToDictionary(
                    g => g.Key,
                    g => g.Count(m => m.Status == StatusMeta.Concluida) / (decimal)g.Count() * 100
                );
        }
    }
} 