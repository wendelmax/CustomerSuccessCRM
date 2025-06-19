using Microsoft.AspNetCore.Mvc;
using CustomerSuccessCRM.Lib.Services;
using CustomerSuccessCRM.Lib.Models;
using CustomerSuccessCRM.Web.ViewModels;

namespace CustomerSuccessCRM.Web.Controllers
{
    public class ClientesController(ClienteService clienteService, ILogger<ClientesController> logger)
        : Controller
    {
        // GET: Clientes
        public async Task<IActionResult> Index(string searchTerm, StatusCliente? status)
        {
            try
            {
                List<Cliente> clientes;

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    // Implementar busca por nome quando disponível
                    clientes = await clienteService.ListarTodosAsync();
                    clientes = clientes.Where(c => c.Nome.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                                  c.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
                }
                else if (status.HasValue)
                {
                    clientes = await clienteService.BuscarPorStatusAsync(status.Value);
                }
                else
                {
                    clientes = await clienteService.ListarTodosAsync();
                }

                ViewBag.SearchTerm = searchTerm;
                ViewBag.SelectedStatus = status;
                ViewBag.StatusList = Enum.GetValues(typeof(StatusCliente))
                    .Cast<StatusCliente>()
                    .Select(s => new { Id = (int)s, Name = s.ToString() });

                return View(clientes);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao listar clientes");
                return View("Error", new ErrorViewModel 
                { 
                    RequestId = HttpContext.TraceIdentifier,
                    Message = "Ocorreu um erro ao listar os clientes. Por favor, tente novamente."
                });
            }
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var cliente = await clienteService.BuscarPorIdAsync(id);
                if (cliente == null)
                {
                    return NotFound();
                }

                return View(cliente);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao buscar detalhes do cliente {ClienteId}", id);
                return View("Error", new ErrorViewModel 
                { 
                    RequestId = HttpContext.TraceIdentifier,
                    Message = "Ocorreu um erro ao buscar os detalhes do cliente. Por favor, tente novamente."
                });
            }
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            ViewBag.StatusList = Enum.GetValues(typeof(StatusCliente))
                .Cast<StatusCliente>()
                .Select(s => new { Id = (int)s, Name = s.ToString() });
            return View();
        }

        // POST: Clientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nome,Email,Telefone,Empresa,VendedorId,Status")] Cliente cliente)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await clienteService.CadastrarAsync(cliente);
                    TempData["Success"] = "Cliente criado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao criar cliente");
                ModelState.AddModelError("", "Erro ao criar cliente: " + ex.Message);
            }

            ViewBag.StatusList = Enum.GetValues(typeof(StatusCliente))
                .Cast<StatusCliente>()
                .Select(s => new { Id = (int)s, Name = s.ToString() });
            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var cliente = await clienteService.BuscarPorIdAsync(id);
                if (cliente == null)
                {
                    return NotFound();
                }

                ViewBag.StatusList = Enum.GetValues(typeof(StatusCliente))
                    .Cast<StatusCliente>()
                    .Select(s => new { Id = (int)s, Name = s.ToString() });
                return View(cliente);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao buscar cliente {ClienteId} para edição", id);
                return View("Error", new ErrorViewModel 
                { 
                    RequestId = HttpContext.TraceIdentifier,
                    Message = "Ocorreu um erro ao buscar os dados do cliente. Por favor, tente novamente."
                });
            }
        }

        // POST: Clientes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Email,Telefone,Empresa,VendedorId,Status")] Cliente cliente)
        {
            try
            {
                if (id != cliente.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    await clienteService.AtualizarAsync(cliente);
                    TempData["Success"] = "Cliente atualizado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao atualizar cliente {ClienteId}", id);
                ModelState.AddModelError("", "Erro ao atualizar cliente: " + ex.Message);
            }

            ViewBag.StatusList = Enum.GetValues(typeof(StatusCliente))
                .Cast<StatusCliente>()
                .Select(s => new { Id = (int)s, Name = s.ToString() });
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var cliente = await clienteService.BuscarPorIdAsync(id);
                if (cliente == null)
                {
                    return NotFound();
                }

                return View(cliente);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao buscar cliente {ClienteId} para exclusão", id);
                return View("Error", new ErrorViewModel 
                { 
                    RequestId = HttpContext.TraceIdentifier,
                    Message = "Ocorreu um erro ao buscar os dados do cliente. Por favor, tente novamente."
                });
            }
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await clienteService.DeletarAsync(id);
                TempData["Success"] = "Cliente excluído com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao excluir cliente {ClienteId}", id);
                return View("Error", new ErrorViewModel 
                { 
                    RequestId = HttpContext.TraceIdentifier,
                    Message = "Ocorreu um erro ao excluir o cliente. Por favor, tente novamente."
                });
            }
        }

        // GET: Clientes/Search
        [HttpGet]
        public async Task<IActionResult> Search(string term)
        {
            try
            {
                if (string.IsNullOrEmpty(term))
                {
                    return Json(new List<Cliente>());
                }

                var clientes = await clienteService.ListarTodosAsync();
                var resultados = clientes.Where(c => 
                    c.Nome.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    c.Email.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    c.Empresa.Contains(term, StringComparison.OrdinalIgnoreCase))
                    .Take(10)
                    .Select(c => new { c.Id, c.Nome, c.Email, c.Empresa });

                return Json(resultados);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao buscar clientes");
                return Json(new List<object>());
            }
        }
    }
} 