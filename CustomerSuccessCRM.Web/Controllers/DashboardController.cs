using Microsoft.AspNetCore.Mvc;
using CustomerSuccessCRM.Lib.Services;
using CustomerSuccessCRM.Lib.Models;
using CustomerSuccessCRM.Web.ViewModels;

namespace CustomerSuccessCRM.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ClienteService _clienteService;
        private readonly ProdutoService _produtoService;
        private readonly MetaService _metaService;

        public DashboardController(
            ClienteService clienteService,
            ProdutoService produtoService,
            MetaService metaService)
        {
            _clienteService = clienteService;
            _produtoService = produtoService;
            _metaService = metaService;
        }

        public async Task<IActionResult> Index()
        {
            var dashboard = new CrmDashboard
            {
                TotalClientes = await _clienteService.ContarClientesAtivosAsync(),
                TotalProdutos = (await _produtoService.ListarTodosAsync()).Count,
                TotalMetas = (await _metaService.ListarTodasAsync()).Count,
                TotalInteracoes = 0, // Removido do escopo simplificado

                ClientesRecentes = await _clienteService.ListarTodosAsync(),
                ProdutosMaisVendidos = await _produtoService.ListarTodosAsync(),

                ValorTotalMetas = (await _metaService.ListarTodasAsync())
                    .Sum(m => m.Valor),

                MetasConcluidas = (await _metaService.ListarTodasAsync())
                    .Count(m => m.Status == Lib.Models.StatusMeta.Concluida),

                MetasEmAndamento = (await _metaService.ListarTodasAsync())
                    .Count(m => m.Status == Lib.Models.StatusMeta.EmAndamento)
            };

            return View(dashboard);
        }

        public IActionResult Relatorios()
        {
            TempData["Info"] = "O módulo de Relatórios está em desenvolvimento. Em breve estará disponível!";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Configuracoes()
        {
            TempData["Info"] = "O módulo de Configurações está em desenvolvimento. Em breve estará disponível!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> GetDashboardData()
        {
            try
            {
                var dashboard = await GetDashboardDataAsync();
                return Json(dashboard);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        private async Task<CrmDashboard> GetDashboardDataAsync()
        {
            var dashboard = new CrmDashboard();

            // Buscar dados básicos
            var clientes = await _clienteService.ListarTodosAsync();
            var produtos = await _produtoService.ListarTodosAsync();
            var metas = await _metaService.ListarTodasAsync();

            // Preencher dados do dashboard
            dashboard.TotalClientes = clientes.Count;
            dashboard.TotalProdutos = produtos.Count;
            dashboard.TotalMetas = metas.Count;
            dashboard.TotalInteracoes = 0; // Será implementado quando tivermos InteracaoService

            // Calcular dados das metas
            dashboard.MetasConcluidas = metas.Count(m => m.Status == StatusMeta.Concluida);
            dashboard.MetasEmAndamento = metas.Count(m => m.Status == StatusMeta.EmAndamento);
            dashboard.ValorTotalMetas = metas.Sum(m => m.Valor);

            // Buscar produtos mais vendidos
            var produtosMaisVendidos = await _produtoService.ListarMaisVendidosAsync(5);
            dashboard.ProdutosMaisVendidos = produtosMaisVendidos;

            // Buscar clientes recentes
            dashboard.ClientesRecentes = clientes.OrderByDescending(c => c.DataCadastro).Take(5).ToList();

            return dashboard;
        }
    }
} 