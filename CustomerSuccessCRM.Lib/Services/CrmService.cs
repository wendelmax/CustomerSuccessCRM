using System.Linq.Expressions;
using CustomerSuccessCRM.Lib.Models;
using CustomerSuccessCRM.Lib.Repositories;

namespace CustomerSuccessCRM.Lib.Services
{
    public class CrmService : ICrmService
    {
        private readonly IRepository<Cliente> _clienteRepository;
        private readonly IRepository<Interacao> _interacaoRepository;
        private readonly IRepository<Oportunidade> _oportunidadeRepository;
        private readonly IRepository<Produto> _produtoRepository;
        private readonly NotificationService _notificationService;
        private readonly EmailService _emailService;

        public CrmService(
            IRepository<Cliente> clienteRepository,
            IRepository<Interacao> interacaoRepository,
            IRepository<Oportunidade> oportunidadeRepository,
            IRepository<Produto> produtoRepository,
            NotificationService notificationService,
            EmailService emailService)
        {
            _clienteRepository = clienteRepository;
            _interacaoRepository = interacaoRepository;
            _oportunidadeRepository = oportunidadeRepository;
            _produtoRepository = produtoRepository;
            _notificationService = notificationService;
            _emailService = emailService;
        }

        // Operações de Cliente
        public async Task<IEnumerable<Cliente>> GetAllClientesAsync()
        {
            return await _clienteRepository.GetAllWithIncludesAsync(
                c => c.Interacoes,
                c => c.Oportunidades);
        }

        public async Task<Cliente?> GetClienteByIdAsync(int id)
        {
            return await _clienteRepository.GetByIdWithIncludesAsync(id,
                c => c.Interacoes,
                c => c.Oportunidades);
        }

        public async Task<Cliente> CreateClienteAsync(Cliente cliente)
        {
            cliente.DataCadastro = DateTime.Now;
            cliente.UltimaAtualizacao = DateTime.Now;
            return await _clienteRepository.AddAsync(cliente);
        }

        public async Task<Cliente> UpdateClienteAsync(Cliente cliente)
        {
            cliente.UltimaAtualizacao = DateTime.Now;
            return await _clienteRepository.UpdateAsync(cliente);
        }

        public async Task DeleteClienteAsync(int id)
        {
            await _clienteRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Cliente>> SearchClientesAsync(string searchTerm)
        {
            return await _clienteRepository.FindWithIncludesAsync(
                c => c.Nome.Contains(searchTerm) || 
                     c.Email.Contains(searchTerm) || 
                     c.Telefone.Contains(searchTerm),
                c => c.Interacoes,
                c => c.Oportunidades);
        }

        public async Task<IEnumerable<Cliente>> GetClientesByStatusAsync(StatusCliente status)
        {
            return await _clienteRepository.FindWithIncludesAsync(
                c => c.Status == status,
                c => c.Interacoes,
                c => c.Oportunidades);
        }

        // Operações de Interação
        public async Task<IEnumerable<Interacao>> GetAllInteracoesAsync()
        {
            return await _interacaoRepository.GetAllWithIncludesAsync(
                i => i.Cliente,
                i => i.Responsavel);
        }

        public async Task<Interacao?> GetInteracaoByIdAsync(int id)
        {
            return await _interacaoRepository.GetByIdWithIncludesAsync(id,
                i => i.Cliente,
                i => i.Responsavel);
        }

        public async Task<Interacao> CreateInteracaoAsync(Interacao interacao)
        {
            interacao.DataInteracao = DateTime.Now;
            var result = await _interacaoRepository.AddAsync(interacao);
            
            if (result != null)
            {
                var cliente = await _clienteRepository.GetByIdAsync(interacao.ClienteId);
                
                // Notificar sobre nova interação
                await _notificationService.SendNotificationAsync(
                    interacao.Responsavel,
                    "Nova Interação Registrada",
                    $"Uma nova interação foi registrada para o cliente {cliente?.Nome}");

                // Enviar email de acompanhamento
                if (cliente != null)
                {
                    await _emailService.SendEmailAsync(
                        cliente.Email,
                        "Acompanhamento de Atendimento",
                        $"Olá {cliente.Nome},\n\nRegistramos sua interação conosco sobre: {interacao.Assunto}");
                }
            }

            return result;
        }

        public async Task<Interacao> UpdateInteracaoAsync(Interacao interacao)
        {
            return await _interacaoRepository.UpdateAsync(interacao);
        }

        public async Task DeleteInteracaoAsync(int id)
        {
            await _interacaoRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Interacao>> GetInteracoesByClienteIdAsync(int clienteId)
        {
            return await _interacaoRepository.FindWithIncludesAsync(
                i => i.ClienteId == clienteId,
                i => i.Cliente,
                i => i.Responsavel);
        }

        public async Task<IEnumerable<Interacao>> GetInteracoesPendentesAsync()
        {
            return await _interacaoRepository.FindWithIncludesAsync(
                i => i.Status == StatusInteracao.Pendente,
                i => i.Cliente,
                i => i.Responsavel);
        }

        // Operações de Oportunidade
        public async Task<IEnumerable<Oportunidade>> GetAllOportunidadesAsync()
        {
            return await _oportunidadeRepository.GetAllWithIncludesAsync(
                o => o.Cliente,
                o => o.Responsavel,
                o => o.Produtos);
        }

        public async Task<Oportunidade?> GetOportunidadeByIdAsync(int id)
        {
            return await _oportunidadeRepository.GetByIdWithIncludesAsync(id,
                o => o.Cliente,
                o => o.Responsavel,
                o => o.Produtos);
        }

        public async Task<Oportunidade> CreateOportunidadeAsync(Oportunidade oportunidade)
        {
            oportunidade.DataCriacao = DateTime.Now;
            var result = await _oportunidadeRepository.AddAsync(oportunidade);
            
            if (result != null)
            {
                var cliente = await _clienteRepository.GetByIdAsync(oportunidade.ClienteId);
                
                // Notificar sobre nova oportunidade
                await _notificationService.SendNotificationAsync(
                    oportunidade.Responsavel,
                    "Nova Oportunidade Registrada",
                    $"Uma nova oportunidade foi registrada para o cliente {cliente?.Nome}");
            }

            return result;
        }

        public async Task<Oportunidade> UpdateOportunidadeAsync(Oportunidade oportunidade)
        {
            return await _oportunidadeRepository.UpdateAsync(oportunidade);
        }

        public async Task DeleteOportunidadeAsync(int id)
        {
            await _oportunidadeRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Oportunidade>> GetOportunidadesByClienteIdAsync(int clienteId)
        {
            return await _oportunidadeRepository.FindWithIncludesAsync(
                o => o.ClienteId == clienteId,
                o => o.Cliente,
                o => o.Responsavel,
                o => o.Produtos);
        }

        public async Task<IEnumerable<Oportunidade>> GetOportunidadesAbertasAsync()
        {
            return await _oportunidadeRepository.FindWithIncludesAsync(
                o => o.Fase == FaseOportunidade.Aberta || o.Fase == FaseOportunidade.Negociacao,
                o => o.Cliente,
                o => o.Responsavel,
                o => o.Produtos);
        }

        // Operações de Produto
        public async Task<IEnumerable<Produto>> GetAllProdutosAsync()
        {
            return await _produtoRepository.GetAllWithIncludesAsync(
                p => p.Variacoes,
                p => p.Precos,
                p => p.Bundles);
        }

        public async Task<Produto?> GetProdutoByIdAsync(int id)
        {
            return await _produtoRepository.GetByIdWithIncludesAsync(id,
                p => p.Variacoes,
                p => p.Precos,
                p => p.Bundles);
        }

        public async Task<Produto> CreateProdutoAsync(Produto produto)
        {
            produto.DataCadastro = DateTime.Now;
            produto.DataAtualizacao = DateTime.Now;
            return await _produtoRepository.AddAsync(produto);
        }

        public async Task<Produto> UpdateProdutoAsync(Produto produto)
        {
            produto.DataAtualizacao = DateTime.Now;
            return await _produtoRepository.UpdateAsync(produto);
        }

        public async Task DeleteProdutoAsync(int id)
        {
            await _produtoRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Produto>> GetProdutosAtivosAsync()
        {
            return await _produtoRepository.FindWithIncludesAsync(
                p => p.Ativo,
                p => p.Variacoes,
                p => p.Precos,
                p => p.Bundles);
        }
    }
} 