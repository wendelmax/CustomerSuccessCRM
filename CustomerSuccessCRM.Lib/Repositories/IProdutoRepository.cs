using CustomerSuccessCRM.Lib.Models;

namespace CustomerSuccessCRM.Lib.Repositories
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<IEnumerable<Produto>> GetByCategoria(string categoria);
        Task<IEnumerable<Produto>> GetByFornecedor(string fornecedor);
        Task<IEnumerable<Produto>> GetComEstoqueBaixo(int limiteEstoque);
        Task<IEnumerable<Produto>> GetMaisVendidos(int quantidade);
        Task<decimal> GetPrecoMedio();
        Task<bool> AtualizarEstoque(int id, int quantidade);
        Task<bool> AtualizarPreco(int id, decimal novoPreco);
    }
} 