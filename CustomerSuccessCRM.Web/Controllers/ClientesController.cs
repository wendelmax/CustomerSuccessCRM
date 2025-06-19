using Microsoft.AspNetCore.Mvc;
using CustomerSuccessCRM.Lib.Services;
using CustomerSuccessCRM.Lib.Models;
using CustomerSuccessCRM.Web.Models;

namespace CustomerSuccessCRM.Web.Controllers
{
    public class ClientesController : Controller
    {
        private readonly ICrmService _crmService;

        public ClientesController(ICrmService crmService)
        {
            _crmService = crmService;
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
                return View(clientes);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
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
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
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
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Erro ao criar cliente: " + ex.Message);
            }

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

                return View(cliente);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
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
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Erro ao atualizar cliente: " + ex.Message);
            }

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
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
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
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
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
                return Json(clientes);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
} 