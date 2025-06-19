using CustomerSuccessCRM.Lib.Models;

namespace CustomerSuccessCRM.Lib.Repositories
{
    public interface IOportunidadeRepository : IRepository<Oportunidade>
    {
        Task<IEnumerable<Oportunidade>> GetByClienteIdAsync(int clienteId);
        Task<IEnumerable<Oportunidade>> GetByFaseAsync(FaseOportunidade fase);
        Task<IEnumerable<Oportunidade>> GetByResponsavelAsync(string responsavel);
        Task<IEnumerable<Oportunidade>> GetByDataRangeAsync(DateTime dataInicio, DateTime dataFim);
        Task<IEnumerable<Oportunidade>> GetOportunidadesAbertasAsync();
        Task<IEnumerable<Oportunidade>> GetOportunidadesFechadasAsync();
        Task<decimal> GetValorTotalOportunidadesAsync();
        Task<decimal> GetValorTotalOportunidadesFechadasAsync();
        Task<int> GetTotalOportunidadesAsync();
        Task<decimal> GetTaxaConversaoAsync();
    }
} 