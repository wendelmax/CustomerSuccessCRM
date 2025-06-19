using CustomerSuccessCRM.Lib.Models;
using CustomerSuccessCRM.Lib.Repositories;

namespace CustomerSuccessCRM.Lib.Services
{
    public class ProdutoService
    {
        private readonly IProdutoRepository _repository;

        public ProdutoService(IProdutoRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Produto>> ListarTodosAsync()
        {
            return await _repository.BuscarTodosAsync();
        }

        public async Task<Produto> BuscarPorIdAsync(int id)
        {
            return await _repository.BuscarPorIdAsync(id);
        }

        public async Task<bool> CadastrarAsync(Produto produto)
        {
            if (string.IsNullOrEmpty(produto.Nome))
                throw new ArgumentException("Nome do produto é obrigatório");

            if (produto.PrecoBase <= 0)
                throw new ArgumentException("Preço do produto deve ser maior que zero");

            produto.DataCadastro = DateTime.Now;
            produto.Ativo = true;

            return await _repository.AdicionarAsync(produto);
        }

        public async Task<bool> AtualizarAsync(Produto produto)
        {
            var produtoExistente = await _repository.BuscarPorIdAsync(produto.Id);
            if (produtoExistente == null)
                throw new KeyNotFoundException($"Produto com ID {produto.Id} não encontrado");

            produtoExistente.Nome = produto.Nome;
            produtoExistente.Descricao = produto.Descricao;
            produtoExistente.PrecoBase = produto.PrecoBase;
            produtoExistente.Categoria = produto.Categoria;
            produtoExistente.QuantidadeEstoque = produto.QuantidadeEstoque;
            produtoExistente.DataAtualizacao = DateTime.Now;

            return await _repository.AtualizarAsync(produtoExistente);
        }

        public async Task<bool> DeletarAsync(int id)
        {
            return await _repository.DeletarAsync(id);
        }

        public async Task<bool> AtualizarPrecoAsync(int id, decimal novoPreco)
        {
            if (novoPreco <= 0)
                throw new ArgumentException("Preço deve ser maior que zero");

            var produto = await _repository.BuscarPorIdAsync(id);
            if (produto == null)
                return false;

            produto.PrecoBase = novoPreco;
            produto.DataAtualizacao = DateTime.Now;

            return await _repository.AtualizarAsync(produto);
        }

        public async Task<bool> AtualizarEstoqueAsync(int id, int quantidade)
        {
            if (quantidade < 0)
                throw new ArgumentException("Quantidade não pode ser negativa");

            var produto = await _repository.BuscarPorIdAsync(id);
            if (produto == null)
                return false;

            produto.QuantidadeEstoque = quantidade;
            produto.DataAtualizacao = DateTime.Now;

            return await _repository.AtualizarAsync(produto);
        }

        public async Task<List<Produto>> BuscarPorCategoriaAsync(CategoriaProduto categoria)
        {
            return await _repository.BuscarPorCategoriaAsync(categoria);
        }

        public async Task<List<Produto>> BuscarPorFaixaPrecoAsync(decimal precoMinimo, decimal precoMaximo)
        {
            return await _repository.BuscarPorFaixaPrecoAsync(precoMinimo, precoMaximo);
        }

        public async Task<List<Produto>> ListarMaisVendidosAsync(int quantidade = 10)
        {
            return await _repository.BuscarMaisVendidosAsync(quantidade);
        }

        public async Task<decimal> CalcularValorTotalEstoqueAsync()
        {
            return await _repository.CalcularValorTotalEstoqueAsync();
        }
    }
} 