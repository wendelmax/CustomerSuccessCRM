using Microsoft.EntityFrameworkCore;
using CustomerSuccessCRM.Lib.Data;
using CustomerSuccessCRM.Lib.Models;

namespace CustomerSuccessCRM.Lib.Repositories
{
    public class OportunidadeRepository : Repository<Oportunidade>, IOportunidadeRepository
    {
        public OportunidadeRepository(CrmDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Oportunidade>> GetAllAsync()
        {
            return await _dbSet
                .Include(o => o.Cliente)
                .OrderByDescending(o => o.DataCriacao)
                .ToListAsync();
        }

        public override async Task<Oportunidade?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(o => o.Cliente)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<Oportunidade>> GetByClienteIdAsync(int clienteId)
        {
            return await _dbSet
                .Where(o => o.ClienteId == clienteId)
                .OrderByDescending(o => o.DataCriacao)
                .ToListAsync();
        }

        public async Task<IEnumerable<Oportunidade>> GetByFaseAsync(FaseOportunidade fase)
        {
            return await _dbSet
                .Include(o => o.Cliente)
                .Where(o => o.Fase == fase)
                .OrderByDescending(o => o.DataCriacao)
                .ToListAsync();
        }

        public async Task<IEnumerable<Oportunidade>> GetByResponsavelAsync(string responsavel)
        {
            return await _dbSet
                .Include(o => o.Cliente)
                .Where(o => o.Responsavel != null && o.Responsavel.Contains(responsavel))
                .OrderByDescending(o => o.DataCriacao)
                .ToListAsync();
        }

        public async Task<IEnumerable<Oportunidade>> GetByDataRangeAsync(DateTime dataInicio, DateTime dataFim)
        {
            return await _dbSet
                .Include(o => o.Cliente)
                .Where(o => o.DataCriacao >= dataInicio && o.DataCriacao <= dataFim)
                .OrderByDescending(o => o.DataCriacao)
                .ToListAsync();
        }

        public async Task<IEnumerable<Oportunidade>> GetOportunidadesAbertasAsync()
        {
            return await _dbSet
                .Include(o => o.Cliente)
                .Where(o => o.Fase != FaseOportunidade.Fechada && o.Fase != FaseOportunidade.Perdida)
                .OrderByDescending(o => o.DataCriacao)
                .ToListAsync();
        }

        public async Task<IEnumerable<Oportunidade>> GetOportunidadesFechadasAsync()
        {
            return await _dbSet
                .Include(o => o.Cliente)
                .Where(o => o.Fase == FaseOportunidade.Fechada)
                .OrderByDescending(o => o.DataFechamento)
                .ToListAsync();
        }

        public async Task<decimal> GetValorTotalOportunidadesAsync()
        {
            var oportunidades = await _dbSet.ToListAsync();
            return oportunidades.Sum(o => o.ValorEstimado);
        }

        public async Task<decimal> GetValorTotalOportunidadesFechadasAsync()
        {
            var oportunidades = await _dbSet
                .Where(o => o.Fase == FaseOportunidade.Fechada)
                .ToListAsync();
            return oportunidades.Sum(o => o.ValorReal);
        }

        public async Task<int> GetTotalOportunidadesAsync()
        {
            return await _dbSet.CountAsync();
        }

        public async Task<decimal> GetTaxaConversaoAsync()
        {
            var totalOportunidades = await _dbSet.CountAsync();
            var oportunidadesFechadas = await _dbSet
                .Where(o => o.Fase == FaseOportunidade.Fechada)
                .CountAsync();

            if (totalOportunidades == 0) return 0;

            return (decimal)oportunidadesFechadas / totalOportunidades * 100;
        }
    }
} 