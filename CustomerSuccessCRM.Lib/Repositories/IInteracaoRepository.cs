using CustomerSuccessCRM.Lib.Models;

namespace CustomerSuccessCRM.Lib.Repositories
{
    public interface IInteracaoRepository : IRepository<Interacao>
    {
        Task<IEnumerable<Interacao>> GetByClienteIdAsync(int clienteId);
        Task<IEnumerable<Interacao>> GetByTipoAsync(TipoInteracao tipo);
        Task<IEnumerable<Interacao>> GetByStatusAsync(StatusInteracao status);
        Task<IEnumerable<Interacao>> GetByResponsavelAsync(string responsavel);
        Task<IEnumerable<Interacao>> GetByDataRangeAsync(DateTime dataInicio, DateTime dataFim);
        Task<IEnumerable<Interacao>> GetInteracoesPendentesAsync();
        Task<IEnumerable<Interacao>> GetInteracoesUrgentesAsync();
        Task<int> GetTotalInteracoesAsync(int clienteId);
    }
} 