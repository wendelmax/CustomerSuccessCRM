using CustomerSuccessCRM.Lib.Models;

namespace CustomerSuccessCRM.Lib.Repositories
{
    public interface IClienteRepository : IRepository<Cliente>
    {
        Task<IEnumerable<Cliente>> GetByStatusAsync(StatusCliente status);
        Task<IEnumerable<Cliente>> GetByEmpresaAsync(string empresa);
        Task<IEnumerable<Cliente>> GetByCidadeAsync(string cidade);
        Task<Cliente?> GetByEmailAsync(string email);
        Task<IEnumerable<Cliente>> SearchAsync(string searchTerm);
        Task<IEnumerable<Cliente>> GetClientesVipAsync();
        Task<IEnumerable<Cliente>> GetProspectosAsync();
        Task<int> GetTotalClientesAsync();
        Task<decimal> GetValorTotalOportunidadesAsync(int clienteId);
    }
} 