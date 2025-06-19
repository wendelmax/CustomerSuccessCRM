using CustomerSuccessCRM.Lib.Models;
using CustomerSuccessCRM.Lib.Services.Contracts;
using CustomerSuccessCRM.Lib.Configuration;
using CustomerSuccessCRM.Lib.Repositories;
using Microsoft.Extensions.Options;

namespace CustomerSuccessCRM.Lib.Services.Strategies
{
    public interface IBusinessRuleStrategy
    {
        Task<bool> ValidatePropostaAsync(Proposta proposta);
        Task<bool> ValidateContratoAsync(Contrato contrato);
        Task<bool> ValidateMetaAsync(Meta meta);
        Task<bool> ValidateWorkflowAsync(Workflow workflow);
        Task<bool> ValidateDescontoAsync(decimal valorOriginal, decimal valorDesconto);
        Task<bool> ValidateEstoqueAsync(int produtoId, int quantidade);
        Task<bool> ValidateBundleAsync(int produtoPrincipalId, IEnumerable<int> produtosRelacionadosIds);
    }

    public class BusinessRuleStrategy : IBusinessRuleStrategy
    {
        private readonly CustomerSuccessCRM.Lib.Configuration.ProdutoSettings _produtoSettings;
        private readonly IProdutoRepository _produtoRepository;

        public BusinessRuleStrategy(
            IOptions<CustomerSuccessCRM.Lib.Configuration.ProdutoSettings> produtoSettings,
            IProdutoRepository produtoRepository)
        {
            _produtoSettings = produtoSettings.Value;
            _produtoRepository = produtoRepository;
        }

        public async Task<bool> ValidatePropostaAsync(Proposta proposta)
        {
            if (proposta == null)
                return false;

            // Validar dados básicos
            if (string.IsNullOrEmpty(proposta.Titulo) || proposta.ClienteId <= 0)
                return false;

            // Validar itens
            if (!proposta.Itens.Any())
                return false;

            foreach (var item in proposta.Itens)
            {
                // Validar produto
                var produto = await _produtoRepository.GetByIdAsync(item.ProdutoId);
                if (produto == null)
                    return false;

                // Validar quantidade
                if (item.Quantidade <= 0)
                    return false;

                // Validar desconto
                if (!await ValidateDescontoAsync(item.ValorUnitario * item.Quantidade, item.DescontoItem))
                    return false;

                // Validar estoque
                if (!await ValidateEstoqueAsync(item.ProdutoId, item.Quantidade))
                    return false;
            }

            // Validar valor total
            var valorTotal = proposta.Itens.Sum(i => i.ValorUnitario * i.Quantidade - i.DescontoItem);
            if (valorTotal != proposta.ValorTotal)
                return false;

            return true;
        }

        public async Task<bool> ValidateContratoAsync(Contrato contrato)
        {
            if (contrato == null)
                return false;

            // Validar dados básicos
            if (string.IsNullOrEmpty(contrato.Titulo) || contrato.ClienteId <= 0)
                return false;

            // Validar datas
            if (contrato.DataVencimento.HasValue && contrato.DataVencimento.Value <= contrato.DataCriacao)
                return false;

            // Validar proposta associada
            if (contrato.PropostaId.HasValue)
            {
                // TODO: Validar se a proposta existe e está aprovada
            }

            // Validar valor
            if (contrato.ValorTotal <= 0)
                return false;

            return true;
        }

        public async Task<bool> ValidateMetaAsync(Meta meta)
        {
            if (meta == null)
                return false;

            // Validar dados básicos
            if (string.IsNullOrEmpty(meta.Titulo) || meta.Valor <= 0)
                return false;

            // Validar datas
            if (meta.DataInicio >= meta.DataFim)
                return false;

            // Validar tipo
            if (!Enum.IsDefined(typeof(TipoMeta), meta.Tipo))
                return false;

            // Validar responsável
            if (string.IsNullOrEmpty(meta.ResponsavelId))
                return false;

            return true;
        }

        public async Task<bool> ValidateWorkflowAsync(Workflow workflow)
        {
            if (workflow == null)
                return false;

            // Validar dados básicos
            if (string.IsNullOrEmpty(workflow.Nome) || string.IsNullOrEmpty(workflow.Descricao))
                return false;

            // Validar tipo
            if (!Enum.IsDefined(typeof(TipoWorkflow), workflow.Tipo))
                return false;

            // Validar condições
            if (workflow.Condicoes != null && workflow.Condicoes.Any())
            {
                foreach (var condicao in workflow.Condicoes)
                {
                    if (string.IsNullOrEmpty(condicao.Campo) || string.IsNullOrEmpty(condicao.Operador))
                        return false;
                }
            }

            // Validar ações
            if (workflow.Acoes != null && workflow.Acoes.Any())
            {
                foreach (var acao in workflow.Acoes)
                {
                    if (string.IsNullOrEmpty(acao.Tipo) || string.IsNullOrEmpty(acao.Parametros))
                        return false;
                }
            }

            return true;
        }

        public async Task<bool> ValidateDescontoAsync(decimal valorOriginal, decimal valorDesconto)
        {
            if (valorOriginal <= 0 || valorDesconto < 0)
                return false;

            // Calcular percentual de desconto
            var percentualDesconto = (valorDesconto / valorOriginal) * 100;

            // Validar se está dentro do limite permitido
            if (percentualDesconto > this._produtoSettings.MaxDiscountPercentage)
                return false;

            return true;
        }

        public async Task<bool> ValidateEstoqueAsync(int produtoId, int quantidade)
        {
            if (produtoId <= 0 || quantidade <= 0)
                return false;

            var produto = await _produtoRepository.GetByIdAsync(produtoId);
            if (produto == null)
                return false;

            // Validar se há estoque suficiente
            if (produto.QuantidadeEstoque < quantidade)
                return false;

            return true;
        }

        public async Task<bool> ValidateBundleAsync(int produtoPrincipalId, IEnumerable<int> produtosRelacionadosIds)
        {
            if (produtoPrincipalId <= 0 || !produtosRelacionadosIds.Any())
                return false;

            // Validar produto principal
            var produtoPrincipal = await _produtoRepository.GetByIdAsync(produtoPrincipalId);
            if (produtoPrincipal == null)
                return false;

            // Validar produtos relacionados
            foreach (var produtoId in produtosRelacionadosIds)
            {
                var produto = await _produtoRepository.GetByIdAsync(produtoId);
                if (produto == null)
                    return false;

                // Validar se o produto não é o mesmo que o principal
                if (produtoId == produtoPrincipalId)
                    return false;
            }

            return true;
        }
    }
} 