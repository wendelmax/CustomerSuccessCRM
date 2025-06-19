using Microsoft.AspNetCore.Mvc;
using CustomerSuccessCRM.Lib.Services;
using CustomerSuccessCRM.Lib.Models;
using CustomerSuccessCRM.Web.Models;

namespace CustomerSuccessCRM.Web.Controllers
{
    public class OportunidadesController : Controller
    {
        private readonly ICrmService _crmService;

        public OportunidadesController(ICrmService crmService)
        {
            _crmService = crmService;
        }

        // GET: Oportunidades
        public async Task<IActionResult> Index(int? clienteId, FaseOportunidade? fase)
        {
            try
            {
                IEnumerable<Oportunidade> oportunidades;

                if (clienteId.HasValue)
                {
                    oportunidades = await _crmService.GetOportunidadesByClienteAsync(clienteId.Value);
                }
                else if (fase.HasValue)
                {
                    // Para simplificar, vamos buscar todas e filtrar no controller
                    var todas = await _crmService.GetAllOportunidadesAsync();
                    oportunidades = todas.Where(o => o.Fase == fase.Value);
                }
                else
                {
                    oportunidades = await _crmService.GetAllOportunidadesAsync();
                }

                ViewBag.ClienteId = clienteId;
                ViewBag.SelectedFase = fase;
                return View(oportunidades);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        // GET: Oportunidades/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var oportunidade = await _crmService.GetOportunidadeByIdAsync(id);
                if (oportunidade == null)
                {
                    return NotFound();
                }

                return View(oportunidade);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        // GET: Oportunidades/Create
        public IActionResult Create(int? clienteId)
        {
            var oportunidade = new Oportunidade();
            if (clienteId.HasValue)
            {
                oportunidade.ClienteId = clienteId.Value;
            }

            ViewBag.ClienteId = clienteId;
            return View(oportunidade);
        }

        // POST: Oportunidades/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClienteId,Titulo,Descricao,ValorEstimado,ValorReal,DataProximoContato,Fase,Probabilidade,Responsavel,Observacoes")] Oportunidade oportunidade)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _crmService.CreateOportunidadeAsync(oportunidade);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Erro ao criar oportunidade: " + ex.Message);
            }

            ViewBag.ClienteId = oportunidade.ClienteId;
            return View(oportunidade);
        }

        // GET: Oportunidades/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var oportunidade = await _crmService.GetOportunidadeByIdAsync(id);
                if (oportunidade == null)
                {
                    return NotFound();
                }

                return View(oportunidade);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        // POST: Oportunidades/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClienteId,Titulo,Descricao,ValorEstimado,ValorReal,DataFechamento,DataProximoContato,Fase,Probabilidade,Responsavel,Observacoes")] Oportunidade oportunidade)
        {
            try
            {
                if (id != oportunidade.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    await _crmService.UpdateOportunidadeAsync(oportunidade);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Erro ao atualizar oportunidade: " + ex.Message);
            }

            return View(oportunidade);
        }

        // GET: Oportunidades/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var oportunidade = await _crmService.GetOportunidadeByIdAsync(id);
                if (oportunidade == null)
                {
                    return NotFound();
                }

                return View(oportunidade);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        // POST: Oportunidades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _crmService.DeleteOportunidadeAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        // GET: Oportunidades/Abertas
        public async Task<IActionResult> Abertas()
        {
            try
            {
                var oportunidades = await _crmService.GetOportunidadesAbertasAsync();
                return View(oportunidades);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }
    }
} 