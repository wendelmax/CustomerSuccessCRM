using CustomerSuccessCRM.Lib.Models;

namespace CustomerSuccessCRM.Lib.Repositories
{
    public interface IMetaRepository : IBaseRepository<Meta>
    {
        Task<List<Meta>> BuscarTodasAsync();
        Task<Meta?> BuscarPorIdAsync(int id);
        Task<List<Meta>> BuscarPorResponsavelAsync(string responsavelId);
        Task<List<Meta>> BuscarPorEquipeAsync(string equipeId);
        Task<List<Meta>> BuscarAtrasadasAsync();
        Task<decimal> CalcularPercentualAtingimentoAsync(string? equipeId = null);
        Task<List<Meta>> BuscarPorPeriodoAsync(DateTime inicio, DateTime fim);
        Task<List<Meta>> BuscarProximasVencerAsync(int dias);
        Task<Dictionary<string, decimal>> CalcularAtingimentoPorEquipeAsync();
    }
} 