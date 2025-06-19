using Microsoft.AspNetCore.Mvc;
using CustomerSuccessCRM.Lib.Services;
using CustomerSuccessCRM.Lib.Models;
using CustomerSuccessCRM.Web.ViewModels;

namespace CustomerSuccessCRM.Web.Controllers
{
    public class DashboardController(
        ClienteService clienteService,
        ProdutoService produtoService,
        MetaService metaService)
        : Controller
    {
        public async Task<IActionResult> Index()
        {
            try
            {
                var dashboard = await GetDashboardDataAsync();
                return View(dashboard);
            }
            catch (Exception ex)
            {
                // Log the exception
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
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
            var clientes = await clienteService.ListarTodosAsync();
            var produtos = await produtoService.ListarTodosAsync();
            var metas = await metaService.ListarTodasAsync();

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
            var produtosMaisVendidos = await produtoService.ListarMaisVendidosAsync(5);
            dashboard.ProdutosMaisVendidos = produtosMaisVendidos;

            // Buscar clientes recentes
            dashboard.ClientesRecentes = clientes.OrderByDescending(c => c.DataCadastro).Take(5).ToList();

            return dashboard;
        }
    }
} 