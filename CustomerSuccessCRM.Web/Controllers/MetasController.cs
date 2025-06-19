using Microsoft.AspNetCore.Mvc;
using CustomerSuccessCRM.Lib.Services;
using CustomerSuccessCRM.Lib.Models;
using CustomerSuccessCRM.Web.ViewModels;

namespace CustomerSuccessCRM.Web.Controllers
{
    public class MetasController(MetaService metaService, ILogger<MetasController> logger) : Controller
    {
        // GET: Metas
        public async Task<IActionResult> Index(string searchTerm, StatusMeta? status)
        {
            try
            {
                List<Meta> metas;

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    // Buscar por nome quando disponível
                    metas = await metaService.ListarTodasAsync();
                    metas = metas.Where(m => m.Nome.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                            m.Descricao.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
                }
                else if (status.HasValue)
                {
                    // Filtrar por status
                    metas = await metaService.ListarTodasAsync();
                    metas = metas.Where(m => m.Status == status.Value).ToList();
                }
                else
                {
                    metas = await metaService.ListarTodasAsync();
                }

                ViewBag.SearchTerm = searchTerm;
                ViewBag.SelectedStatus = status;
                ViewBag.StatusList = Enum.GetValues(typeof(StatusMeta))
                    .Cast<StatusMeta>()
                    .Select(s => new { Id = (int)s, Name = s.ToString() });

                return View(metas);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao listar metas");
                return View("Error", new ErrorViewModel 
                { 
                    RequestId = HttpContext.TraceIdentifier,
                    Message = "Ocorreu um erro ao listar as metas. Por favor, tente novamente."
                });
            }
        }

        // GET: Metas/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var meta = await metaService.BuscarPorIdAsync(id);
                if (meta == null)
                {
                    return NotFound();
                }

                return View(meta);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao buscar detalhes da meta {MetaId}", id);
                return View("Error", new ErrorViewModel 
                { 
                    RequestId = HttpContext.TraceIdentifier,
                    Message = "Ocorreu um erro ao buscar os detalhes da meta. Por favor, tente novamente."
                });
            }
        }

        // GET: Metas/Create
        public IActionResult Create()
        {
            ViewBag.StatusList = Enum.GetValues(typeof(StatusMeta))
                .Cast<StatusMeta>()
                .Select(s => new { Id = (int)s, Name = s.ToString() });
            return View();
        }

        // POST: Metas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nome,Descricao,Valor,ResponsavelId,EquipeId,DataInicio,DataFim")] Meta meta)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await metaService.CadastrarAsync(meta);
                    TempData["Success"] = "Meta criada com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao criar meta");
                ModelState.AddModelError("", "Erro ao criar meta: " + ex.Message);
            }

            ViewBag.StatusList = Enum.GetValues(typeof(StatusMeta))
                .Cast<StatusMeta>()
                .Select(s => new { Id = (int)s, Name = s.ToString() });
            return View(meta);
        }

        // GET: Metas/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var meta = await metaService.BuscarPorIdAsync(id);
                if (meta == null)
                {
                    return NotFound();
                }

                ViewBag.StatusList = Enum.GetValues(typeof(StatusMeta))
                    .Cast<StatusMeta>()
                    .Select(s => new { Id = (int)s, Name = s.ToString() });
                return View(meta);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao buscar meta {MetaId} para edição", id);
                return View("Error", new ErrorViewModel 
                { 
                    RequestId = HttpContext.TraceIdentifier,
                    Message = "Ocorreu um erro ao buscar os dados da meta. Por favor, tente novamente."
                });
            }
        }

        // POST: Metas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Descricao,Valor,Progresso,ResponsavelId,EquipeId,Status,DataInicio,DataFim")] Meta meta)
        {
            try
            {
                if (id != meta.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    await metaService.AtualizarAsync(meta);
                    TempData["Success"] = "Meta atualizada com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao atualizar meta {MetaId}", id);
                ModelState.AddModelError("", "Erro ao atualizar meta: " + ex.Message);
            }

            ViewBag.StatusList = Enum.GetValues(typeof(StatusMeta))
                .Cast<StatusMeta>()
                .Select(s => new { Id = (int)s, Name = s.ToString() });
            return View(meta);
        }

        // GET: Metas/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var meta = await metaService.BuscarPorIdAsync(id);
                if (meta == null)
                {
                    return NotFound();
                }

                return View(meta);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao buscar meta {MetaId} para exclusão", id);
                return View("Error", new ErrorViewModel 
                { 
                    RequestId = HttpContext.TraceIdentifier,
                    Message = "Ocorreu um erro ao buscar os dados da meta. Por favor, tente novamente."
                });
            }
        }

        // POST: Metas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await metaService.DeletarAsync(id);
                TempData["Success"] = "Meta excluída com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao excluir meta {MetaId}", id);
                return View("Error", new ErrorViewModel 
                { 
                    RequestId = HttpContext.TraceIdentifier,
                    Message = "Ocorreu um erro ao excluir a meta. Por favor, tente novamente."
                });
            }
        }

        // GET: Metas/Atrasadas
        public async Task<IActionResult> Atrasadas()
        {
            try
            {
                var metas = await metaService.ListarAtrasadasAsync();
                return View(metas);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao listar metas atrasadas");
                return View("Error", new ErrorViewModel 
                { 
                    RequestId = HttpContext.TraceIdentifier,
                    Message = "Ocorreu um erro ao listar as metas atrasadas. Por favor, tente novamente."
                });
            }
        }

        // GET: Metas/ProximasVencer
        public async Task<IActionResult> ProximasVencer()
        {
            try
            {
                // Buscar todas as metas e filtrar as próximas de vencer
                var todasMetas = await metaService.ListarTodasAsync();
                var dataLimite = DateTime.Today.AddDays(7);
                var metas = todasMetas.Where(m => m.Status == StatusMeta.EmAndamento && m.DataFim <= dataLimite).ToList();
                
                return View(metas);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao listar metas próximas de vencer");
                return View("Error", new ErrorViewModel 
                { 
                    RequestId = HttpContext.TraceIdentifier,
                    Message = "Ocorreu um erro ao listar as metas próximas de vencer. Por favor, tente novamente."
                });
            }
        }

        // POST: Metas/AtualizarProgresso/5
        [HttpPost]
        public async Task<IActionResult> AtualizarProgresso(int id, decimal progresso)
        {
            try
            {
                await metaService.AtualizarProgressoAsync(id, progresso);
                return Json(new { success = true, message = "Progresso atualizado com sucesso!" });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao atualizar progresso da meta {MetaId}", id);
                return Json(new { success = false, message = "Erro ao atualizar progresso: " + ex.Message });
            }
        }
    }
} 