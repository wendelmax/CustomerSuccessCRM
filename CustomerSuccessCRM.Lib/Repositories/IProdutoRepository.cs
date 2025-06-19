using CustomerSuccessCRM.Lib.Models;

namespace CustomerSuccessCRM.Lib.Repositories
{
    public interface IProdutoRepository : IBaseRepository
    {
        Task<List<Produto>> BuscarTodosAsync();
        Task<Produto> BuscarPorIdAsync(int id);
        Task<List<Produto>> BuscarPorCategoriaAsync(CategoriaProduto categoria);
        Task<List<Produto>> BuscarPorFaixaPrecoAsync(decimal precoMinimo, decimal precoMaximo);
        Task<List<Produto>> BuscarMaisVendidosAsync(int quantidade);
        Task<decimal> CalcularValorTotalEstoqueAsync();
        Task<Dictionary<CategoriaProduto, int>> CalcularDistribuicaoPorCategoriaAsync();
        Task<List<Produto>> BuscarPorNomeAsync(string nome);
        Task<List<Produto>> BuscarPorPrecoMaximoAsync(decimal precoMaximo);
    }
} 