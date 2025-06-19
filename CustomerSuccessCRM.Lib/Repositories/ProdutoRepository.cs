using CustomerSuccessCRM.Lib.Models;
using CustomerSuccessCRM.Lib.Data;
using Microsoft.EntityFrameworkCore;

namespace CustomerSuccessCRM.Lib.Repositories
{
    public class ProdutoRepository(CrmDbContext context) : Repository<Produto>(context), IProdutoRepository
    {
        public async Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(CategoriaProduto categoria)
        {
            return await _dbSet
                .Where(p => p.Categoria == categoria && p.Ativo)
                .OrderBy(p => p.Nome)
                .ToListAsync();
        }

        public async Task<IEnumerable<Produto>> GetProdutosPorFaixaPrecoAsync(decimal precoMinimo, decimal precoMaximo)
        {
            return await _dbSet
                .Where(p => p.PrecoBase >= precoMinimo && p.PrecoBase <= precoMaximo && p.Ativo)
                .OrderBy(p => p.PrecoBase)
                .ToListAsync();
        }

        public async Task<IEnumerable<Produto>> GetProdutosPorTipoPrecificacaoAsync(TipoPrecificacao tipo)
        {
            return await _dbSet
                .Where(p => p.TipoPrecificacao == tipo && p.Ativo)
                .OrderBy(p => p.Nome)
                .ToListAsync();
        }

        public async Task<IEnumerable<Produto>> GetProdutosBundleAsync(int bundleId)
        {
            var bundle = await _context.ProdutoBundles
                .Include(b => b.Produtos)
                .FirstOrDefaultAsync(b => b.Id == bundleId);

            return bundle?.Produtos ?? new List<Produto>();
        }

        public async Task<IEnumerable<ProdutoVariacao>> GetVariacoesProdutoAsync(int produtoId)
        {
            return await _context.ProdutoVariacoes
                .Where(v => v.ProdutoId == produtoId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProdutoPreco>> GetPrecosProdutoAsync(int produtoId)
        {
            return await _context.ProdutoPrecos
                .Where(p => p.ProdutoId == produtoId)
                .OrderByDescending(p => p.DataInicio)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProdutoBundle>> GetBundlesProdutoAsync(int produtoId)
        {
            return await _context.ProdutoBundles
                .Include(b => b.Produtos)
                .Where(b => b.Produtos.Any(p => p.Id == produtoId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Produto>> GetProdutosComDescontoAsync()
        {
            return await _dbSet
                .Include(p => p.Precos)
                .Where(p => p.Ativo && p.Precos.Any(pr => pr.Produto.PercentualDesconto > 0))
                .ToListAsync();
        }

        public async Task<IEnumerable<Produto>> GetProdutosEmBundleAsync()
        {
            return await _dbSet
                .Include(p => p.Bundles)
                .Where(p => p.Ativo && p.Bundles.Any())
                .ToListAsync();
        }

        public async Task<IDictionary<CategoriaProduto, int>> GetDistribuicaoPorCategoriaAsync()
        {
            return await _dbSet
                .Where(p => p.Ativo)
                .GroupBy(p => p.Categoria)
                .ToDictionaryAsync(
                    g => g.Key,
                    g => g.Count()
                );
        }

        public async Task<decimal> GetValorTotalEmEstoqueAsync()
        {
            var produtos = await _dbSet
                .Where(p => p.Ativo)
                .ToListAsync();

            return produtos.Sum(p => p.PrecoBase * p.QuantidadeEstoque);
        }

        public async Task<IDictionary<string, decimal>> GetHistoricoPrecosProdutoAsync(int produtoId)
        {
            var precos = await _context.ProdutoPrecos
                .Where(p => p.ProdutoId == produtoId)
                .OrderBy(p => p.DataInicio)
                .ToListAsync();

            return precos.ToDictionary(
                p => p.DataInicio.ToString("yyyy-MM-dd"),
                p => p.Valor
            );
        }

        public async Task<IEnumerable<Produto>> GetProdutosMaisVendidosAsync(int quantidade)
        {
            return await _dbSet
                .Include(p => p.QuantidadeVendida)
                .Where(p => p.Ativo)
                .OrderByDescending(p => p.QuantidadeVendida)
                .Take(quantidade)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProdutoBundle>> GetBundlesMaisVendidosAsync(int quantidade)
        {
            return await _dbSet
                .Include(p => p.Bundles)
                .Where(p => p.Ativo)
                .OrderByDescending(p => p.QuantidadeVendida)
                .Take(quantidade)
                .ToListAsync();
                
        }
    }
} 