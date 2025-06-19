using Microsoft.AspNetCore.Mvc;
using CustomerSuccessCRM.Lib.Services;
using CustomerSuccessCRM.Lib.Models;
using CustomerSuccessCRM.Web.Models;

namespace CustomerSuccessCRM.Web.Controllers
{
    public class InteracoesController : Controller
    {
        private readonly ICrmService _crmService;
        private readonly ILogger<InteracoesController> _logger;

        public InteracoesController(ICrmService crmService, ILogger<InteracoesController> logger)
        {
            _crmService = crmService;
            _logger = logger;
        }

        // GET: Interacoes
        public async Task<IActionResult> Index(int? clienteId, TipoInteracao? tipo, StatusInteracao? status)
        {
            try
            {
                IEnumerable<Interacao> interacoes;

                if (clienteId.HasValue)
                {
                    interacoes = await _crmService.GetInteracoesByClienteIdAsync(clienteId.Value);
                    ViewBag.Cliente = await _crmService.GetClienteByIdAsync(clienteId.Value);
                }
                else
                {
                    interacoes = await _crmService.GetAllInteracoesAsync();
                }

                if (tipo.HasValue)
                {
                    interacoes = interacoes.Where(i => i.Tipo == tipo.Value);
                }

                if (status.HasValue)
                {
                    interacoes = interacoes.Where(i => i.Status == status.Value);
                }

                ViewBag.ClienteId = clienteId;
                ViewBag.SelectedTipo = tipo;
                ViewBag.SelectedStatus = status;
                ViewBag.TipoList = Enum.GetValues(typeof(TipoInteracao))
                    .Cast<TipoInteracao>()
                    .Select(t => new { Id = (int)t, Name = t.ToString() });
                ViewBag.StatusList = Enum.GetValues(typeof(StatusInteracao))
                    .Cast<StatusInteracao>()
                    .Select(s => new { Id = (int)s, Name = s.ToString() });

                return View(interacoes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar interações");
                return View("Error", new ErrorViewModel 
                { 
                    RequestId = HttpContext.TraceIdentifier,
                    Message = "Ocorreu um erro ao listar as interações. Por favor, tente novamente."
                });
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
                _logger.LogError(ex, "Erro ao buscar detalhes da interação {InteracaoId}", id);
                return View("Error", new ErrorViewModel 
                { 
                    RequestId = HttpContext.TraceIdentifier,
                    Message = "Ocorreu um erro ao buscar os detalhes da interação. Por favor, tente novamente."
                });
            }
        }

        // GET: Interacoes/Create
        public async Task<IActionResult> Create(int? clienteId)
        {
            var interacao = new Interacao
            {
                DataInteracao = DateTime.Now,
                Status = StatusInteracao.Pendente
            };

            if (clienteId.HasValue)
            {
                var cliente = await _crmService.GetClienteByIdAsync(clienteId.Value);
                if (cliente != null)
                {
                    interacao.ClienteId = cliente.Id;
                    ViewBag.Cliente = cliente;
                }
            }

            ViewBag.ClienteId = clienteId;
            ViewBag.TipoList = Enum.GetValues(typeof(TipoInteracao))
                .Cast<TipoInteracao>()
                .Select(t => new { Id = (int)t, Name = t.ToString() });
            ViewBag.StatusList = Enum.GetValues(typeof(StatusInteracao))
                .Cast<StatusInteracao>()
                .Select(s => new { Id = (int)s, Name = s.ToString() });

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
                    TempData["Success"] = "Interação criada com sucesso!";
                    return RedirectToAction(nameof(Index), new { clienteId = interacao.ClienteId });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar interação");
                ModelState.AddModelError("", "Erro ao criar interação: " + ex.Message);
            }

            if (interacao.ClienteId > 0)
            {
                var cliente = await _crmService.GetClienteByIdAsync(interacao.ClienteId);
                ViewBag.Cliente = cliente;
            }

            ViewBag.ClienteId = interacao.ClienteId;
            ViewBag.TipoList = Enum.GetValues(typeof(TipoInteracao))
                .Cast<TipoInteracao>()
                .Select(t => new { Id = (int)t, Name = t.ToString() });
            ViewBag.StatusList = Enum.GetValues(typeof(StatusInteracao))
                .Cast<StatusInteracao>()
                .Select(s => new { Id = (int)s, Name = s.ToString() });

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

                if (interacao.ClienteId > 0)
                {
                    var cliente = await _crmService.GetClienteByIdAsync(interacao.ClienteId);
                    ViewBag.Cliente = cliente;
                }

                ViewBag.TipoList = Enum.GetValues(typeof(TipoInteracao))
                    .Cast<TipoInteracao>()
                    .Select(t => new { Id = (int)t, Name = t.ToString() });
                ViewBag.StatusList = Enum.GetValues(typeof(StatusInteracao))
                    .Cast<StatusInteracao>()
                    .Select(s => new { Id = (int)s, Name = s.ToString() });

                return View(interacao);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar interação {InteracaoId} para edição", id);
                return View("Error", new ErrorViewModel 
                { 
                    RequestId = HttpContext.TraceIdentifier,
                    Message = "Ocorreu um erro ao buscar os dados da interação. Por favor, tente novamente."
                });
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
                    if (interacao.Status == StatusInteracao.Concluida && !interacao.DataConclusao.HasValue)
                    {
                        interacao.DataConclusao = DateTime.Now;
                    }

                    await _crmService.UpdateInteracaoAsync(interacao);
                    TempData["Success"] = "Interação atualizada com sucesso!";
                    return RedirectToAction(nameof(Index), new { clienteId = interacao.ClienteId });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar interação {InteracaoId}", id);
                ModelState.AddModelError("", "Erro ao atualizar interação: " + ex.Message);
            }

            if (interacao.ClienteId > 0)
            {
                var cliente = await _crmService.GetClienteByIdAsync(interacao.ClienteId);
                ViewBag.Cliente = cliente;
            }

            ViewBag.TipoList = Enum.GetValues(typeof(TipoInteracao))
                .Cast<TipoInteracao>()
                .Select(t => new { Id = (int)t, Name = t.ToString() });
            ViewBag.StatusList = Enum.GetValues(typeof(StatusInteracao))
                .Cast<StatusInteracao>()
                .Select(s => new { Id = (int)s, Name = s.ToString() });

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

                if (interacao.ClienteId > 0)
                {
                    var cliente = await _crmService.GetClienteByIdAsync(interacao.ClienteId);
                    ViewBag.Cliente = cliente;
                }

                return View(interacao);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar interação {InteracaoId} para exclusão", id);
                return View("Error", new ErrorViewModel 
                { 
                    RequestId = HttpContext.TraceIdentifier,
                    Message = "Ocorreu um erro ao buscar os dados da interação. Por favor, tente novamente."
                });
            }
        }

        // POST: Interacoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var interacao = await _crmService.GetInteracaoByIdAsync(id);
                var clienteId = interacao?.ClienteId;

                await _crmService.DeleteInteracaoAsync(id);
                TempData["Success"] = "Interação excluída com sucesso!";
                return RedirectToAction(nameof(Index), new { clienteId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir interação {InteracaoId}", id);
                return View("Error", new ErrorViewModel 
                { 
                    RequestId = HttpContext.TraceIdentifier,
                    Message = "Ocorreu um erro ao excluir a interação. Por favor, tente novamente."
                });
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
                _logger.LogError(ex, "Erro ao listar interações pendentes");
                return View("Error", new ErrorViewModel 
                { 
                    RequestId = HttpContext.TraceIdentifier,
                    Message = "Ocorreu um erro ao listar as interações pendentes. Por favor, tente novamente."
                });
            }
        }
    }
} 