using CustomerSuccessCRM.Lib.Models;

namespace CustomerSuccessCRM.Lib.Repositories
{
    public interface IClienteRepository : IBaseRepository<Cliente>
    {
        Task<List<Cliente>> BuscarTodosAsync();
        Task<Cliente?> BuscarPorIdAsync(int id);
        Task<List<Cliente>> BuscarPorStatusAsync(StatusCliente status);
        Task<List<Cliente>> BuscarPorVendedorAsync(string vendedorId);
        Task<List<Cliente>> BuscarInativos();
        Task<int> ContarClientesAtivosAsync();
    }
} 