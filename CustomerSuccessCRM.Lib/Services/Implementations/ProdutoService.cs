using CustomerSuccessCRM.Lib.Models;
using CustomerSuccessCRM.Lib.Services.Contracts;
using CustomerSuccessCRM.Lib.Services.Strategies;
using CustomerSuccessCRM.Lib.Configuration;
using CustomerSuccessCRM.Lib.Repositories;
using Microsoft.Extensions.Options;

namespace CustomerSuccessCRM.Lib.Services.Implementations
{
    public class ProdutoService : IProdutoService
    {
        private readonly IBusinessRuleStrategy _businessRuleStrategy;
        private readonly ProdutoSettings _produtoSettings;
        private readonly IProdutoRepository _produtoRepository;

        public ProdutoService(
            IBusinessRuleStrategy businessRuleStrategy,
            IOptions<ProdutoSettings> produtoSettings,
            IProdutoRepository produtoRepository)
        {
            _businessRuleStrategy = businessRuleStrategy;
            _produtoSettings = produtoSettings.Value;
            _produtoRepository = produtoRepository;
        }

        public async Task<bool> ValidateDescontoAsync(decimal valorOriginal, decimal valorDesconto)
        {
            return await _businessRuleStrategy.ValidateDescontoAsync(valorOriginal, valorDesconto);
        }

        public async Task<bool> ValidateEstoqueAsync(int produtoId, int quantidade)
        {
            return await _businessRuleStrategy.ValidateEstoqueAsync(produtoId, quantidade);
        }

        public async Task<bool> ValidateBundleAsync(int produtoPrincipalId, IEnumerable<int> produtosRelacionadosIds)
        {
            return await _businessRuleStrategy.ValidateBundleAsync(produtoPrincipalId, produtosRelacionadosIds);
        }

        public async Task<Produto> GetByIdAsync(int id)
        {
            return await _produtoRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Produto>> GetAllAsync()
        {
            return await _produtoRepository.GetAllAsync();
        }

        public async Task<bool> RequereAprovacaoDescontoAsync(decimal valorDesconto)
        {
            return _produtoSettings.RequireApprovalForDiscounts && 
                   valorDesconto > _produtoSettings.MaxDiscountPercentage;
        }
    }
} 