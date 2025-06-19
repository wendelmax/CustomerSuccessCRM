using CustomerSuccessCRM.Lib.Models;
using CustomerSuccessCRM.Lib.Services.Contracts;
using CustomerSuccessCRM.Lib.Repositories;

namespace CustomerSuccessCRM.Lib.Services.Implementations
{
    public class CrmService : ICrmService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IInteracaoRepository _interacaoRepository;
        private readonly IOportunidadeRepository _oportunidadeRepository;
        private readonly IProdutoRepository _produtoRepository;
        private readonly INotificationService _notificationService;
        private readonly IEmailService _emailService;

        public CrmService(
            IClienteRepository clienteRepository,
            IInteracaoRepository interacaoRepository,
            IOportunidadeRepository oportunidadeRepository,
            IProdutoRepository produtoRepository,
            INotificationService notificationService,
            IEmailService emailService)
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
            return await _clienteRepository.GetAllAsync();
        }

        public async Task<Cliente?> GetClienteByIdAsync(int id)
        {
            return await _clienteRepository.GetByIdAsync(id);
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
            return await _clienteRepository.SearchAsync(searchTerm);
        }

        public async Task<IEnumerable<Cliente>> GetClientesByStatusAsync(StatusCliente status)
        {
            return await _clienteRepository.GetByStatusAsync(status);
        }

        // Operações de Interação
        public async Task<IEnumerable<Interacao>> GetAllInteracoesAsync()
        {
            return await _interacaoRepository.GetAllAsync();
        }

        public async Task<Interacao?> GetInteracaoByIdAsync(int id)
        {
            return await _interacaoRepository.GetByIdAsync(id);
        }

        public async Task<Interacao> CreateInteracaoAsync(Interacao interacao)
        {
            interacao.DataInteracao = DateTime.Now;
            var result = await _interacaoRepository.AddAsync(interacao);
            
            if (result != null)
            {
                // Notificar sobre nova interação
                await _notificationService.EnviarNotificacaoAsync(
                    interacao.Responsavel,
                    "Nova Interação Registrada",
                    $"Uma nova interação foi registrada para o cliente {interacao.Cliente?.Nome}");

                // Enviar email de acompanhamento
                await _emailService.EnviarEmailAsync(
                    interacao.Cliente?.Email,
                    "Acompanhamento de Atendimento",
                    $"Olá {interacao.Cliente?.Nome},\n\nRegistramos sua interação conosco sobre: {interacao.Assunto}");
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
            return await _interacaoRepository.GetByClienteIdAsync(clienteId);
        }

        public async Task<IEnumerable<Interacao>> GetInteracoesPendentesAsync()
        {
            return await _interacaoRepository.GetInteracoesPendentesAsync();
        }

        // Operações de Oportunidade
        public async Task<IEnumerable<Oportunidade>> GetAllOportunidadesAsync()
        {
            return await _oportunidadeRepository.GetAllAsync();
        }

        public async Task<Oportunidade?> GetOportunidadeByIdAsync(int id)
        {
            return await _oportunidadeRepository.GetByIdAsync(id);
        }

        public async Task<Oportunidade> CreateOportunidadeAsync(Oportunidade oportunidade)
        {
            oportunidade.DataCriacao = DateTime.Now;
            var result = await _oportunidadeRepository.AddAsync(oportunidade);
            
            if (result != null)
            {
                // Notificar sobre nova oportunidade
                await _notificationService.EnviarNotificacaoAsync(
                    oportunidade.Responsavel,
                    "Nova Oportunidade Registrada",
                    $"Uma nova oportunidade foi registrada para o cliente {oportunidade.Cliente?.Nome}");
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
            return await _oportunidadeRepository.GetByClienteIdAsync(clienteId);
        }

        public async Task<IEnumerable<Oportunidade>> GetOportunidadesAbertasAsync()
        {
            return await _oportunidadeRepository.GetOportunidadesAbertasAsync();
        }

        // Operações de Produto
        public async Task<IEnumerable<Produto>> GetAllProdutosAsync()
        {
            return await _produtoRepository.GetAllAsync();
        }

        public async Task<Produto?> GetProdutoByIdAsync(int id)
        {
            return await _produtoRepository.GetByIdAsync(id);
        }

        public async Task<Produto> CreateProdutoAsync(Produto produto)
        {
            produto.DataCadastro = DateTime.Now;
            produto.UltimaAtualizacao = DateTime.Now;
            return await _produtoRepository.AddAsync(produto);
        }

        public async Task<Produto> UpdateProdutoAsync(Produto produto)
        {
            produto.UltimaAtualizacao = DateTime.Now;
            return await _produtoRepository.UpdateAsync(produto);
        }

        public async Task DeleteProdutoAsync(int id)
        {
            await _produtoRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Produto>> GetProdutosAtivosAsync()
        {
            return await _produtoRepository.GetAtivosAsync();
        }

        // Relatórios e Métricas
        public async Task<CrmDashboard> GetDashboardDataAsync()
        {
            var dashboard = new CrmDashboard();

            // Métricas básicas
            dashboard.TotalClientes = await _clienteRepository.GetTotalClientesAsync();
            dashboard.TotalProspectos = (await _clienteRepository.GetProspectosAsync()).Count();
            dashboard.TotalClientesVip = (await _clienteRepository.GetClientesVipAsync()).Count();
            dashboard.TotalInteracoes = (await _interacaoRepository.GetAllAsync()).Count();
            dashboard.InteracoesPendentes = (await _interacaoRepository.GetInteracoesPendentesAsync()).Count();
            dashboard.TotalOportunidades = await _oportunidadeRepository.GetTotalOportunidadesAsync();
            dashboard.OportunidadesAbertas = (await _oportunidadeRepository.GetOportunidadesAbertasAsync()).Count();
            dashboard.OportunidadesFechadas = (await _oportunidadeRepository.GetOportunidadesFechadasAsync()).Count();
            dashboard.ValorTotalOportunidades = await _oportunidadeRepository.GetValorTotalOportunidadesAsync();
            dashboard.ValorTotalOportunidadesFechadas = await _oportunidadeRepository.GetValorTotalOportunidadesFechadasAsync();
            dashboard.TaxaConversao = await _oportunidadeRepository.GetTaxaConversaoAsync();
            dashboard.TotalProdutos = await _produtoRepository.GetTotalProdutosAsync();
            dashboard.ValorTotalProdutos = await _produtoRepository.GetValorTotalProdutosAsync();

            // Dados recentes
            dashboard.ClientesRecentes = (await _clienteRepository.GetAllAsync()).Take(5).ToList();
            dashboard.InteracoesRecentes = (await _interacaoRepository.GetAllAsync()).Take(5).ToList();
            dashboard.OportunidadesRecentes = (await _oportunidadeRepository.GetAllAsync()).Take(5).ToList();

            return dashboard;
        }
    }
} 