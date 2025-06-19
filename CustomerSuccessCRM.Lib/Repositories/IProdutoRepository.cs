using CustomerSuccessCRM.Lib.Models;

namespace CustomerSuccessCRM.Lib.Repositories
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(CategoriaProduto categoria);
        Task<IEnumerable<Produto>> GetProdutosPorFaixaPrecoAsync(decimal precoMinimo, decimal precoMaximo);
        Task<IEnumerable<Produto>> GetProdutosPorTipoPrecificacaoAsync(TipoPrecificacao tipo);
        Task<IEnumerable<Produto>> GetProdutosBundleAsync(int bundleId);
        Task<IEnumerable<ProdutoVariacao>> GetVariacoesProdutoAsync(int produtoId);
        Task<IEnumerable<ProdutoPreco>> GetPrecosProdutoAsync(int produtoId);
        Task<IEnumerable<ProdutoBundle>> GetBundlesProdutoAsync(int produtoId);
        Task<IEnumerable<Produto>> GetProdutosComDescontoAsync();
        Task<IEnumerable<Produto>> GetProdutosEmBundleAsync();
        Task<IDictionary<CategoriaProduto, int>> GetDistribuicaoPorCategoriaAsync();
        Task<decimal> GetValorTotalEmEstoqueAsync();
        Task<IDictionary<string, decimal>> GetHistoricoPrecosProdutoAsync(int produtoId);
        Task<IEnumerable<Produto>> GetProdutosMaisVendidosAsync(int quantidade);
        Task<IEnumerable<ProdutoBundle>> GetBundlesMaisVendidosAsync(int quantidade);
    }
} 