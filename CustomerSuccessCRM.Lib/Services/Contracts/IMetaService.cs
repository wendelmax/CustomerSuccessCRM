using CustomerSuccessCRM.Lib.Models;

namespace CustomerSuccessCRM.Lib.Services.Contracts
{
    public interface IMetaService
    {
        // Operações básicas
        Task<IEnumerable<Meta>> GetAllMetasAsync();
        Task<Meta?> GetMetaByIdAsync(int id);
        Task<Meta> CreateMetaAsync(Meta meta);
        Task<Meta> UpdateMetaAsync(Meta meta);
        Task DeleteMetaAsync(int id);

        // Operações específicas
        Task<Meta> AtualizarProgressoAsync(int id, decimal novoValor);
        Task<Meta> ConcluirMetaAsync(int id);
        Task<Meta> CancelarMetaAsync(int id, string motivo);
        Task<IEnumerable<Meta>> GetMetasPorResponsavelAsync(string responsavel);
        Task<IEnumerable<Meta>> GetMetasPorEquipeAsync(string equipeId);
        Task<IEnumerable<Meta>> GetMetasPorPeriodoAsync(DateTime inicio, DateTime fim);
        Task<IEnumerable<Meta>> GetMetasAtrasadasAsync();
        Task<IEnumerable<Meta>> GetMetasProximasVencerAsync(int diasAntecedencia = 7);

        // Análises e Relatórios
        Task<decimal> GetPercentualAtingimentoGeralAsync(string? equipeId = null);
        Task<IEnumerable<MetaHistorico>> GetHistoricoMetaAsync(int metaId);
        Task<IDictionary<TipoMeta, decimal>> GetAtingimentoPorTipoAsync();
        Task<IDictionary<string, decimal>> GetAtingimentoPorEquipeAsync();
        Task<IEnumerable<Meta>> GetMetasRecorrentesAsync();

        // Notificações
        Task NotificarMetaAtingidaAsync(int metaId);
        Task NotificarMetaAtrasadaAsync(int metaId);
        Task NotificarProximaVencerAsync(int metaId);
    }
} 