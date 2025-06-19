using Microsoft.AspNetCore.Mvc;
using CustomerSuccessCRM.Lib.Services;
using CustomerSuccessCRM.Lib.Models;
using CustomerSuccessCRM.Web.Models;

namespace CustomerSuccessCRM.Web.Controllers
{
    public class InteracoesController : Controller
    {
        private readonly ICrmService _crmService;

        public InteracoesController(ICrmService crmService)
        {
            _crmService = crmService;
        }

        // GET: Interacoes
        public async Task<IActionResult> Index(int? clienteId, TipoInteracao? tipo, StatusInteracao? status)
        {
            try
            {
                IEnumerable<Interacao> interacoes;

                if (clienteId.HasValue)
                {
                    interacoes = await _crmService.GetInteracoesByClienteAsync(clienteId.Value);
                }
                else
                {
                    interacoes = await _crmService.GetAllInteracoesAsync();
                }

                ViewBag.ClienteId = clienteId;
                ViewBag.SelectedTipo = tipo;
                ViewBag.SelectedStatus = status;
                return View(interacoes);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        // GET: Interacoes/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var interacao = await _crmService.GetInteracaoByIdAsync(id);
                if (interacao == null)
                {
                    return NotFound();
                }

                return View(interacao);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        // GET: Interacoes/Create
        public IActionResult Create(int? clienteId)
        {
            var interacao = new Interacao();
            if (clienteId.HasValue)
            {
                interacao.ClienteId = clienteId.Value;
            }

            ViewBag.ClienteId = clienteId;
            return View(interacao);
        }

        // POST: Interacoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClienteId,Assunto,Descricao,Tipo,Prioridade,Status,Responsavel,Observacoes")] Interacao interacao)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _crmService.CreateInteracaoAsync(interacao);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Erro ao criar interação: " + ex.Message);
            }

            ViewBag.ClienteId = interacao.ClienteId;
            return View(interacao);
        }

        // GET: Interacoes/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var interacao = await _crmService.GetInteracaoByIdAsync(id);
                if (interacao == null)
                {
                    return NotFound();
                }

                return View(interacao);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        // POST: Interacoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClienteId,Assunto,Descricao,Tipo,Prioridade,Status,Responsavel,DataConclusao,Observacoes")] Interacao interacao)
        {
            try
            {
                if (id != interacao.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    await _crmService.UpdateInteracaoAsync(interacao);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Erro ao atualizar interação: " + ex.Message);
            }

            return View(interacao);
        }

        // GET: Interacoes/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var interacao = await _crmService.GetInteracaoByIdAsync(id);
                if (interacao == null)
                {
                    return NotFound();
                }

                return View(interacao);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        // POST: Interacoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _crmService.DeleteInteracaoAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        // GET: Interacoes/Pendentes
        public async Task<IActionResult> Pendentes()
        {
            try
            {
                var interacoes = await _crmService.GetInteracoesPendentesAsync();
                return View(interacoes);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }
    }
} 