using Microsoft.AspNetCore.Mvc;
using CustomerSuccessCRM.Lib.Services;
using CustomerSuccessCRM.Lib.Models;
using CustomerSuccessCRM.Web.Models;

namespace CustomerSuccessCRM.Web.Controllers
{
    public class ClientesController : Controller
    {
        private readonly ICrmService _crmService;
        private readonly ILogger<ClientesController> _logger;

        public ClientesController(ICrmService crmService, ILogger<ClientesController> logger)
        {
            _crmService = crmService;
            _logger = logger;
        }

        // GET: Clientes
        public async Task<IActionResult> Index(string searchTerm, StatusCliente? status)
        {
            try
            {
                IEnumerable<Cliente> clientes;

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    clientes = await _crmService.SearchClientesAsync(searchTerm);
                }
                else if (status.HasValue)
                {
                    clientes = await _crmService.GetClientesByStatusAsync(status.Value);
                }
                else
                {
                    clientes = await _crmService.GetAllClientesAsync();
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
                _logger.LogError(ex, "Erro ao listar clientes");
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
                var cliente = await _crmService.GetClienteByIdAsync(id);
                if (cliente == null)
                {
                    return NotFound();
                }

                return View(cliente);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar detalhes do cliente {ClienteId}", id);
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
        public async Task<IActionResult> Create([Bind("Nome,Sobrenome,Email,Telefone,Empresa,Cargo,Endereco,Cidade,Estado,CEP,Observacoes,Status")] Cliente cliente)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _crmService.CreateClienteAsync(cliente);
                    TempData["Success"] = "Cliente criado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar cliente");
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
                var cliente = await _crmService.GetClienteByIdAsync(id);
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
                _logger.LogError(ex, "Erro ao buscar cliente {ClienteId} para edição", id);
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Sobrenome,Email,Telefone,Empresa,Cargo,Endereco,Cidade,Estado,CEP,Observacoes,Status")] Cliente cliente)
        {
            try
            {
                if (id != cliente.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    await _crmService.UpdateClienteAsync(cliente);
                    TempData["Success"] = "Cliente atualizado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar cliente {ClienteId}", id);
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
                var cliente = await _crmService.GetClienteByIdAsync(id);
                if (cliente == null)
                {
                    return NotFound();
                }

                return View(cliente);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar cliente {ClienteId} para exclusão", id);
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
                await _crmService.DeleteClienteAsync(id);
                TempData["Success"] = "Cliente excluído com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir cliente {ClienteId}", id);
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

                var clientes = await _crmService.SearchClientesAsync(term);
                var result = clientes.Select(c => new
                {
                    id = c.Id,
                    text = $"{c.Nome} {c.Sobrenome} - {c.Empresa}",
                    nome = c.Nome,
                    sobrenome = c.Sobrenome,
                    email = c.Email,
                    empresa = c.Empresa
                });
                return Json(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao pesquisar clientes com o termo: {SearchTerm}", term);
                return BadRequest(new { error = "Ocorreu um erro ao pesquisar clientes." });
            }
        }
    }
} 