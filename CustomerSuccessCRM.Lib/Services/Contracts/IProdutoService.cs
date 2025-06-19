using CustomerSuccessCRM.Lib.Models;

namespace CustomerSuccessCRM.Lib.Services.Contracts
{
    public interface IProdutoService
    {
        // Operações básicas
        Task<IEnumerable<Produto>> GetAllProdutosAsync();
        Task<Produto?> GetProdutoByIdAsync(int id);
        Task<Produto> CreateProdutoAsync(Produto produto);
        Task<Produto> UpdateProdutoAsync(Produto produto);
        Task DeleteProdutoAsync(int id);

        // Operações de Variações
        Task<ProdutoVariacao> AddVariacaoAsync(int produtoId, ProdutoVariacao variacao);
        Task<ProdutoVariacao> UpdateVariacaoAsync(ProdutoVariacao variacao);
        Task RemoveVariacaoAsync(int variacaoId);
        Task<IEnumerable<ProdutoVariacao>> GetVariacoesProdutoAsync(int produtoId);

        // Operações de Preços
        Task<ProdutoPreco> AddPrecoAsync(int produtoId, ProdutoPreco preco);
        Task<ProdutoPreco> UpdatePrecoAsync(ProdutoPreco preco);
        Task RemovePrecoAsync(int precoId);
        Task<IEnumerable<ProdutoPreco>> GetPrecosProdutoAsync(int produtoId);
        Task<decimal> CalcularPrecoFinalAsync(int produtoId, IDictionary<string, object> condicoes);

        // Operações de Bundles
        Task<ProdutoBundle> AddBundleAsync(int produtoId, ProdutoBundle bundle);
        Task<ProdutoBundle> UpdateBundleAsync(ProdutoBundle bundle);
        Task RemoveBundleAsync(int bundleId);
        Task<IEnumerable<ProdutoBundle>> GetBundlesProdutoAsync(int produtoId);
        Task<decimal> CalcularPrecoBundleAsync(int bundleId, IDictionary<string, object> condicoes);

        // Operações de Desconto
        Task<bool> ValidarDescontoAsync(int produtoId, decimal valorDesconto);
        Task<decimal> CalcularDescontoMaximoAsync(int produtoId, IDictionary<string, object> condicoes);
        Task<bool> RequererAprovacaoDescontoAsync(int produtoId, decimal valorDesconto);

        // Consultas Específicas
        Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(CategoriaProduto categoria);
        Task<IEnumerable<Produto>> GetProdutosPorTipoPrecificacaoAsync(TipoPrecificacao tipo);
        Task<IEnumerable<Produto>> SearchProdutosAsync(string termo);
        Task<IEnumerable<Produto>> GetProdutosComDescontoAsync();
        Task<IEnumerable<Produto>> GetProdutosEmBundleAsync();

        // Análise e Relatórios
        Task<IDictionary<CategoriaProduto, int>> GetDistribuicaoPorCategoriaAsync();
        Task<decimal> GetValorTotalEmEstoqueAsync();
        Task<IDictionary<string, decimal>> GetHistoricoPrecosProdutoAsync(int produtoId);
        Task<IEnumerable<Produto>> GetProdutosMaisVendidosAsync(int quantidade = 10);
        Task<IEnumerable<ProdutoBundle>> GetBundlesMaisVendidosAsync(int quantidade = 10);
    }
} 