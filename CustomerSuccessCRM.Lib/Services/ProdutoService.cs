using CustomerSuccessCRM.Lib.Configuration;
using CustomerSuccessCRM.Lib.Models;
using CustomerSuccessCRM.Lib.Repositories;
using Microsoft.Extensions.Options;

namespace CustomerSuccessCRM.Lib.Services
{
    public class ProdutoService
    {
        private readonly IProdutoRepository _repository;
        private readonly NotificationService _notificationService;
        private readonly ProdutoSettings _settings;

        public ProdutoService(
            IProdutoRepository repository,
            NotificationService notificationService,
            IOptions<ProdutoSettings> settings)
        {
            _repository = repository;
            _notificationService = notificationService;
            _settings = settings.Value;
        }

        public async Task<Produto> CriarProdutoAsync(Produto produto)
        {
            if (produto == null)
                throw new ArgumentNullException(nameof(produto));

            if (string.IsNullOrEmpty(produto.Nome))
                throw new ArgumentException("Nome do produto é obrigatório");

            if (produto.PrecoBase <= 0)
                throw new ArgumentException("Preço do produto deve ser maior que zero");

            produto.DataCadastro = DateTime.UtcNow;
            produto.Ativo = true;

            var novoProduto = await _repository.AddAsync(produto);

            await _notificationService.SendNotificationAsync(
                "admin",
                "Novo Produto Cadastrado",
                $"O produto {produto.Nome} foi cadastrado com sucesso.");

            return novoProduto;
        }

        public async Task<Produto> AtualizarProdutoAsync(Produto produto)
        {
            if (produto == null)
                throw new ArgumentNullException(nameof(produto));

            var produtoExistente = await _repository.GetByIdAsync(produto.Id);
            if (produtoExistente == null)
                throw new KeyNotFoundException($"Produto com ID {produto.Id} não encontrado");

            produtoExistente.Nome = produto.Nome;
            produtoExistente.Descricao = produto.Descricao;
            produtoExistente.PrecoBase = produto.PrecoBase;
            produtoExistente.Categoria = produto.Categoria;
            produtoExistente.QuantidadeEstoque = produto.QuantidadeEstoque;
            produtoExistente.Fornecedor = produto.Fornecedor;
            produtoExistente.DataAtualizacao = DateTime.UtcNow;

            var produtoAtualizado = await _repository.UpdateAsync(produtoExistente);

            await _notificationService.SendNotificationAsync(
                "admin",
                "Produto Atualizado",
                $"O produto {produto.Nome} foi atualizado com sucesso.");

            return produtoAtualizado;
        }

        public async Task<bool> AtualizarPrecoAsync(int produtoId, decimal novoPreco)
        {
            if (novoPreco <= 0)
                throw new ArgumentException("Preço deve ser maior que zero");

            var produto = await _repository.GetByIdAsync(produtoId);
            if (produto == null)
                return false;

            produto.PrecoBase = novoPreco;
            produto.DataAtualizacao = DateTime.UtcNow;

            await _repository.UpdateAsync(produto);

            await _notificationService.SendNotificationAsync(
                "admin",
                "Preço de Produto Atualizado",
                $"O preço do produto {produto.Nome} foi atualizado para {novoPreco:C}");

            return true;
        }

        public async Task<bool> AtualizarEstoqueAsync(int produtoId, int quantidade)
        {
            if (quantidade < 0)
                throw new ArgumentException("Quantidade não pode ser negativa");

            var produto = await _repository.GetByIdAsync(produtoId);
            if (produto == null)
                return false;

            produto.QuantidadeEstoque = quantidade;
            produto.DataAtualizacao = DateTime.UtcNow;

            await _repository.UpdateAsync(produto);

            if (quantidade <= _settings.EstoqueMinimoAlerta)
            {
                await _notificationService.SendNotificationAsync(
                    "admin",
                    "Alerta de Estoque Baixo",
                    $"O produto {produto.Nome} está com estoque baixo: {quantidade} unidades");
            }

            return true;
        }

        public async Task<bool> DesativarProdutoAsync(int produtoId)
        {
            var produto = await _repository.GetByIdAsync(produtoId);
            if (produto == null)
                return false;

            produto.Ativo = false;
            produto.DataAtualizacao = DateTime.UtcNow;

            await _repository.UpdateAsync(produto);

            await _notificationService.SendNotificationAsync(
                "admin",
                "Produto Desativado",
                $"O produto {produto.Nome} foi desativado");

            return true;
        }

        public async Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(CategoriaProduto categoria)
        {
            return await _repository.GetProdutosPorCategoriaAsync(categoria);
        }

        public async Task<IEnumerable<Produto>> GetProdutosPorFaixaPrecoAsync(decimal precoMinimo, decimal precoMaximo)
        {
            return await _repository.GetProdutosPorFaixaPrecoAsync(precoMinimo, precoMaximo);
        }

        public async Task<IEnumerable<Produto>> GetProdutosPorTipoPrecificacaoAsync(TipoPrecificacao tipo)
        {
            return await _repository.GetProdutosPorTipoPrecificacaoAsync(tipo);
        }

        public async Task<decimal> CalcularPrecoComDescontoAsync(int produtoId, decimal percentualDesconto)
        {
            var produto = await _repository.GetByIdAsync(produtoId);
            if (produto == null)
                throw new KeyNotFoundException($"Produto com ID {produtoId} não encontrado");

            if (percentualDesconto > _settings.PercentualDescontoMaximo)
                throw new InvalidOperationException($"Desconto máximo permitido é {_settings.PercentualDescontoMaximo}%");

            return produto.PrecoBase * (1 - percentualDesconto / 100);
        }

        public async Task<IEnumerable<Produto>> GetProdutosBundleAsync(int bundleId)
        {
            return await _repository.GetProdutosBundleAsync(bundleId);
        }

        public async Task<decimal> CalcularPrecoTotalBundleAsync(int bundleId)
        {
            var produtos = await GetProdutosBundleAsync(bundleId);
            return produtos.Sum(p => p.PrecoBase);
        }

        public async Task<IEnumerable<Produto>> GetAllProdutosAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Produto?> GetProdutoByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Produto> CreateProdutoAsync(Produto produto)
        {
            return await _repository.AddAsync(produto);
        }

        public async Task<Produto> UpdateProdutoAsync(Produto produto)
        {
            return await _repository.UpdateAsync(produto);
        }

        public async Task DeleteProdutoAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<ProdutoVariacao> AddVariacaoAsync(int produtoId, ProdutoVariacao variacao)
        {
            var produto = await _repository.GetByIdAsync(produtoId);
            if (produto == null)
                throw new InvalidOperationException("Produto não encontrado");

            variacao.ProdutoId = produtoId;
            return await _repository.AddAsync(variacao);
        }

        public async Task<ProdutoVariacao> UpdateVariacaoAsync(ProdutoVariacao variacao)
        {
            return await _repository.UpdateAsync(variacao);
        }

        public async Task RemoveVariacaoAsync(int variacaoId)
        {
            await _repository.DeleteAsync(variacaoId);
        }

        public async Task<IEnumerable<ProdutoVariacao>> GetVariacoesProdutoAsync(int produtoId)
        {
            return await _repository.GetVariacoesProdutoAsync(produtoId);
        }

        public async Task<ProdutoPreco> AddPrecoAsync(int produtoId, Decimal preco)
        {
            var produto = await _repository.GetByIdAsync(produtoId);
            if (produto == null)
                throw new InvalidOperationException("Produto não encontrado");

            produto.PrecoBase = preco;
            return await _repository.AddAsync(produto);
        }

        public async Task<ProdutoPreco> UpdatePrecoAsync(ProdutoPreco preco)
        {
            return await _repository.UpdateAsync(preco);
        }

        public async Task RemovePrecoAsync(int precoId)
        {
            await _repository.DeleteAsync(precoId);
        }

        public async Task<IEnumerable<ProdutoPreco>> GetPrecosProdutoAsync(int produtoId)
        {
            return await _repository.GetPrecosProdutoAsync(produtoId);
        }

        public async Task<decimal> CalcularPrecoFinalAsync(int produtoId, IDictionary<string, object> parametros)
        {
            var produto = await _repository.GetByIdAsync(produtoId);
            if (produto == null)
                throw new InvalidOperationException("Produto não encontrado");

            var precoBase = produto.PrecoBase;
            var precoFinal = precoBase;

            // Aplicar regras de precificação
            if (parametros.TryGetValue("quantidade", out var quantidadeObj) && 
                int.TryParse(quantidadeObj.ToString(), out var quantidade))
            {
                // Desconto por quantidade
                if (quantidade >= _settings.QuantidadeDescontoMinima)
                {
                    precoFinal *= (1 - _settings.PercentualDescontoQuantidade / 100m);
                }
            }

            if (parametros.TryGetValue("cliente_vip", out var clienteVipObj) && 
                bool.TryParse(clienteVipObj.ToString(), out var clienteVip) && 
                clienteVip)
            {
                // Desconto VIP
                precoFinal *= (1 - _settings.PercentualDescontoVip / 100m);
            }

            return Math.Round(precoFinal, 2);
        }

        public async Task<ProdutoBundle> AddBundleAsync(int produtoId, ProdutoBundle bundle)
        {
            var produto = await _repository.GetByIdAsync(produtoId);
            if (produto == null)
                throw new InvalidOperationException("Produto não encontrado");

            bundle.ProdutoId = produtoId;
            return await _repository.AddAsync(bundle);
        }

        public async Task<ProdutoBundle> UpdateBundleAsync(ProdutoBundle bundle)
        {
            return await _repository.UpdateAsync(bundle);
        }

        public async Task RemoveBundleAsync(int bundleId)
        {
            await _repository.DeleteAsync(bundleId);
        }

        public async Task<IEnumerable<ProdutoBundle>> GetBundlesProdutoAsync(int produtoId)
        {
            return await _repository.GetBundlesProdutoAsync(produtoId);
        }

        public async Task<decimal> CalcularPrecoBundleAsync(int bundleId, IDictionary<string, object> parametros)
        {
            var produtos = await GetProdutosBundleAsync(bundleId);
            var precoTotal = 0m;

            foreach (var produto in produtos)
            {
                var precoFinal = await CalcularPrecoFinalAsync(produto.Id, parametros);
                precoTotal += precoFinal;
            }

            // Aplicar desconto do bundle
            if (_settings.PercentualDescontoBundle > 0)
            {
                precoTotal *= (1 - _settings.PercentualDescontoBundle / 100m);
            }

            return Math.Round(precoTotal, 2);
        }

        public async Task<bool> ValidarDescontoAsync(int produtoId, decimal percentualDesconto)
        {
            var produto = await _repository.GetByIdAsync(produtoId);
            return produto != null && percentualDesconto <= _settings.PercentualDescontoMaximo;
        }

        public async Task<decimal> CalcularDescontoMaximoAsync(int produtoId, IDictionary<string, object> parametros)
        {
            var produto = await _repository.GetByIdAsync(produtoId);
            if (produto == null)
                throw new InvalidOperationException("Produto não encontrado");

            var descontoMaximo = _settings.PercentualDescontoMaximo;

            // Regras adicionais de desconto
            if (parametros.TryGetValue("cliente_vip", out var clienteVipObj) && 
                bool.TryParse(clienteVipObj.ToString(), out var clienteVip) && 
                clienteVip)
            {
                descontoMaximo += _settings.PercentualDescontoVip;
            }

            if (parametros.TryGetValue("quantidade", out var quantidadeObj) && 
                int.TryParse(quantidadeObj.ToString(), out var quantidade) && 
                quantidade >= _settings.QuantidadeDescontoMinima)
            {
                descontoMaximo += _settings.PercentualDescontoQuantidade;
            }

            if (produto.QuantidadeEstoque > _settings.EstoqueMaximoDesconto)
            {
                descontoMaximo += _settings.PercentualDescontoEstoqueAlto;
            }

            return Math.Min(descontoMaximo, 100m);
        }

        public async Task<bool> RequererAprovacaoDescontoAsync(int produtoId, decimal percentualDesconto)
        {
            var produto = await _repository.GetByIdAsync(produtoId);
            if (produto == null)
                throw new InvalidOperationException("Produto não encontrado");

            if (percentualDesconto > _settings.PercentualDescontoMaximoSemAprovacao)
            {
                await _notificationService.SendNotificationAsync(
                    "admin",
                    "Aprovação de Desconto Necessária",
                    $"O produto {produto.Nome} requer aprovação para desconto de {percentualDesconto}%");
                return true;
            }

            return false;
        }

        public async Task<IEnumerable<Produto>> SearchProdutosAsync(string termo)
        {
            return await _repository.SearchAsync(p => 
                p.Nome.Contains(termo) || 
                p.Descricao.Contains(termo) || 
                p.Fornecedor.Contains(termo));
        }

        public async Task<IEnumerable<Produto>> GetProdutosComDescontoAsync()
        {
            return await _repository.GetProdutosComDescontoAsync();
        }

        public async Task<IEnumerable<Produto>> GetProdutosEmBundleAsync()
        {
            return await _repository.GetProdutosEmBundleAsync();
        }

        public async Task<IDictionary<CategoriaProduto, int>> GetDistribuicaoPorCategoriaAsync()
        {
            return await _repository.GetDistribuicaoPorCategoriaAsync();
        }

        public async Task<decimal> GetValorTotalEmEstoqueAsync()
        {
            return await _repository.GetValorTotalEmEstoqueAsync();
        }

        public async Task<IDictionary<string, decimal>> GetHistoricoPrecosProdutoAsync(int produtoId)
        {
            return await _repository.GetHistoricoPrecosProdutoAsync(produtoId);
        }

        public async Task<IEnumerable<Produto>> GetProdutosMaisVendidosAsync(int quantidade)
        {
            return await _repository.GetProdutosMaisVendidosAsync(quantidade);
        }

        public async Task<IEnumerable<ProdutoBundle>> GetBundlesMaisVendidosAsync(int quantidade)
        {
            return await _repository.GetBundlesMaisVendidosAsync(quantidade);
        }
    }
} 