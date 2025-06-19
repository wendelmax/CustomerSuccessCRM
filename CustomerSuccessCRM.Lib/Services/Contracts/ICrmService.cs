using CustomerSuccessCRM.Lib.Models;

namespace CustomerSuccessCRM.Lib.Services.Contracts
{
    public interface ICrmService
    {
        // Operações de Cliente
        Task<IEnumerable<Cliente>> GetAllClientesAsync();
        Task<Cliente?> GetClienteByIdAsync(int id);
        Task<Cliente> CreateClienteAsync(Cliente cliente);
        Task<Cliente> UpdateClienteAsync(Cliente cliente);
        Task DeleteClienteAsync(int id);
        Task<IEnumerable<Cliente>> SearchClientesAsync(string searchTerm);
        Task<IEnumerable<Cliente>> GetClientesByStatusAsync(StatusCliente status);

        // Operações de Interação
        Task<IEnumerable<Interacao>> GetAllInteracoesAsync();
        Task<Interacao?> GetInteracaoByIdAsync(int id);
        Task<Interacao> CreateInteracaoAsync(Interacao interacao);
        Task<Interacao> UpdateInteracaoAsync(Interacao interacao);
        Task DeleteInteracaoAsync(int id);
        Task<IEnumerable<Interacao>> GetInteracoesByClienteIdAsync(int clienteId);
        Task<IEnumerable<Interacao>> GetInteracoesPendentesAsync();

        // Operações de Oportunidade
        Task<IEnumerable<Oportunidade>> GetAllOportunidadesAsync();
        Task<Oportunidade?> GetOportunidadeByIdAsync(int id);
        Task<Oportunidade> CreateOportunidadeAsync(Oportunidade oportunidade);
        Task<Oportunidade> UpdateOportunidadeAsync(Oportunidade oportunidade);
        Task DeleteOportunidadeAsync(int id);
        Task<IEnumerable<Oportunidade>> GetOportunidadesByClienteIdAsync(int clienteId);
        Task<IEnumerable<Oportunidade>> GetOportunidadesAbertasAsync();

        // Operações de Produto
        Task<IEnumerable<Produto>> GetAllProdutosAsync();
        Task<Produto?> GetProdutoByIdAsync(int id);
        Task<Produto> CreateProdutoAsync(Produto produto);
        Task<Produto> UpdateProdutoAsync(Produto produto);
        Task DeleteProdutoAsync(int id);
        Task<IEnumerable<Produto>> GetProdutosAtivosAsync();

        // Relatórios e Métricas
        Task<CrmDashboard> GetDashboardDataAsync();
    }
} 