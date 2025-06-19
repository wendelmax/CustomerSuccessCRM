using CustomerSuccessCRM.Lib.Models;

namespace CustomerSuccessCRM.Lib.Repositories
{
    public interface IMetaRepository : IRepository<Meta>
    {
        Task<IEnumerable<Meta>> GetMetasByResponsavelAsync(string responsavelId);
        Task<IEnumerable<Meta>> GetMetasByEquipeAsync(string equipeId);
        Task<IEnumerable<Meta>> GetMetasByPeriodoAsync(DateTime inicio, DateTime fim);
        Task<IEnumerable<Meta>> GetMetasAtrasadasAsync();
        Task<IEnumerable<Meta>> GetMetasProximasVencerAsync(int dias);
        Task<decimal> GetPercentualAtingimentoGeralAsync(string? equipeId = null);
        Task<IEnumerable<MetaHistorico>> GetHistoricoMetaAsync(int metaId);
        Task<IDictionary<TipoMeta, decimal>> GetAtingimentoPorTipoAsync();
        Task<IDictionary<string, decimal>> GetAtingimentoPorEquipeAsync();
        Task<IEnumerable<Meta>> GetMetasRecorrentesAsync();
    }
} 