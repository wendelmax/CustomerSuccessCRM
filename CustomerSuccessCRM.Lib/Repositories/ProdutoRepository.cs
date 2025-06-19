using CustomerSuccessCRM.Lib.Models;
using CustomerSuccessCRM.Lib.Data;
using Microsoft.EntityFrameworkCore;

namespace CustomerSuccessCRM.Lib.Repositories
{
    public class ProdutoRepository : BaseRepository, IProdutoRepository
    {
        private readonly DbSet<Produto> _produtos;

        public ProdutoRepository(CrmDbContext context) : base(context, "Produtos")
        {
            _produtos = context.Set<Produto>();
        }

        public override async Task<bool> ExisteAsync(int id)
        {
            return await _produtos.FindAsync(id) != null;
        }

        public override async Task<bool> AdicionarAsync(object entidade)
        {
            if (entidade is Produto produto)
            {
                await _produtos.AddAsync(produto);
                return await SaveChangesAsync();
            }
            return false;
        }

        public override async Task<bool> AtualizarAsync(object entidade)
        {
            if (entidade is Produto produto)
            {
                _produtos.Update(produto);
                return await SaveChangesAsync();
            }
            return false;
        }

        public override async Task<bool> DeletarAsync(int id)
        {
            var produto = await _produtos.FindAsync(id);
            if (produto == null) return false;

            _produtos.Remove(produto);
            return await SaveChangesAsync();
        }

        public async Task<List<Produto>> BuscarTodosAsync()
        {
            return await _produtos
                .Where(p => p.Ativo)
                .ToListAsync();
        }

        public async Task<Produto> BuscarPorIdAsync(int id)
        {
            return await _produtos.FindAsync(id);
        }

        public async Task<List<Produto>> BuscarPorCategoriaAsync(CategoriaProduto categoria)
        {
            return await _produtos
                .Where(p => p.Categoria == categoria && p.Ativo)
                .OrderBy(p => p.Nome)
                .ToListAsync();
        }

        public async Task<List<Produto>> BuscarPorFaixaPrecoAsync(decimal precoMinimo, decimal precoMaximo)
        {
            return await _produtos
                .Where(p => p.PrecoBase >= precoMinimo && p.PrecoBase <= precoMaximo && p.Ativo)
                .OrderBy(p => p.PrecoBase)
                .ToListAsync();
        }

        public async Task<List<Produto>> BuscarMaisVendidosAsync(int quantidade)
        {
            return await _produtos
                .Where(p => p.Ativo)
                .OrderByDescending(p => p.QuantidadeVendida)
                .Take(quantidade)
                .ToListAsync();
        }

        public async Task<decimal> CalcularValorTotalEstoqueAsync()
        {
            var produtos = await _produtos
                .Where(p => p.Ativo)
                .ToListAsync();

            return produtos.Sum(p => p.PrecoBase * p.QuantidadeEstoque);
        }

        public async Task<Dictionary<CategoriaProduto, int>> CalcularDistribuicaoPorCategoriaAsync()
        {
            return await _produtos
                .Where(p => p.Ativo)
                .GroupBy(p => p.Categoria)
                .ToDictionaryAsync(
                    g => g.Key,
                    g => g.Count()
                );
        }

        public async Task<List<Produto>> BuscarPorNomeAsync(string nome)
        {
            return await _produtos
                .Where(p => p.Nome.Contains(nome) && p.Ativo)
                .OrderBy(p => p.Nome)
                .ToListAsync();
        }

        public async Task<List<Produto>> BuscarPorPrecoMaximoAsync(decimal precoMaximo)
        {
            return await _produtos
                .Where(p => p.PrecoBase <= precoMaximo && p.Ativo)
                .OrderBy(p => p.PrecoBase)
                .ToListAsync();
        }
    }
} 