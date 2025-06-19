using Microsoft.EntityFrameworkCore;
using CustomerSuccessCRM.Lib.Data;
using CustomerSuccessCRM.Lib.Models;

namespace CustomerSuccessCRM.Lib.Repositories
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(CrmDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Produto>> GetAllAsync()
        {
            return await _dbSet
                .OrderBy(p => p.Nome)
                .ToListAsync();
        }

        public async Task<IEnumerable<Produto>> GetByCategoriaAsync(CategoriaProduto categoria)
        {
            return await _dbSet
                .Where(p => p.Categoria == categoria)
                .OrderBy(p => p.Nome)
                .ToListAsync();
        }

        public async Task<IEnumerable<Produto>> GetAtivosAsync()
        {
            return await _dbSet
                .Where(p => p.Ativo)
                .OrderBy(p => p.Nome)
                .ToListAsync();
        }

        public async Task<IEnumerable<Produto>> GetByPrecoRangeAsync(decimal precoMin, decimal precoMax)
        {
            return await _dbSet
                .Where(p => p.Preco >= precoMin && p.Preco <= precoMax)
                .OrderBy(p => p.Preco)
                .ToListAsync();
        }

        public async Task<IEnumerable<Produto>> SearchAsync(string searchTerm)
        {
            var term = searchTerm.ToLower();
            return await _dbSet
                .Where(p => p.Nome.ToLower().Contains(term) ||
                           (p.Descricao != null && p.Descricao.ToLower().Contains(term)) ||
                           (p.Codigo != null && p.Codigo.ToLower().Contains(term)))
                .OrderBy(p => p.Nome)
                .ToListAsync();
        }

        public async Task<Produto?> GetByCodigoAsync(string codigo)
        {
            return await _dbSet
                .FirstOrDefaultAsync(p => p.Codigo == codigo);
        }

        public async Task<int> GetTotalProdutosAsync()
        {
            return await _dbSet.CountAsync();
        }

        public async Task<decimal> GetValorTotalProdutosAsync()
        {
            var produtos = await _dbSet.ToListAsync();
            return produtos.Sum(p => p.Preco);
        }
    }
} 