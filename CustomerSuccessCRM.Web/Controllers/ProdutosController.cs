using Microsoft.AspNetCore.Mvc;
using CustomerSuccessCRM.Lib.Services;
using CustomerSuccessCRM.Lib.Models;
using CustomerSuccessCRM.Web.Models;

namespace CustomerSuccessCRM.Web.Controllers
{
    public class ProdutosController : Controller
    {
        private readonly ICrmService _crmService;

        public ProdutosController(ICrmService crmService)
        {
            _crmService = crmService;
        }

        // GET: Produtos
        public async Task<IActionResult> Index(CategoriaProduto? categoria, string searchTerm)
        {
            try
            {
                IEnumerable<Produto> produtos;

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    // Para simplificar, vamos buscar todos e filtrar no controller
                    var todos = await _crmService.GetAllProdutosAsync();
                    produtos = todos.Where(p => p.Nome.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                               (p.Descricao != null && p.Descricao.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)));
                }
                else if (categoria.HasValue)
                {
                    // Para simplificar, vamos buscar todos e filtrar no controller
                    var todos = await _crmService.GetAllProdutosAsync();
                    produtos = todos.Where(p => p.Categoria == categoria.Value);
                }
                else
                {
                    produtos = await _crmService.GetAllProdutosAsync();
                }

                ViewBag.SelectedCategoria = categoria;
                ViewBag.SearchTerm = searchTerm;
                return View(produtos);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        // GET: Produtos/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var produto = await _crmService.GetProdutoByIdAsync(id);
                if (produto == null)
                {
                    return NotFound();
                }

                return View(produto);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        // GET: Produtos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Produtos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nome,Descricao,Preco,Codigo,Categoria,Observacoes")] Produto produto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _crmService.CreateProdutoAsync(produto);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Erro ao criar produto: " + ex.Message);
            }

            return View(produto);
        }

        // GET: Produtos/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var produto = await _crmService.GetProdutoByIdAsync(id);
                if (produto == null)
                {
                    return NotFound();
                }

                return View(produto);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        // POST: Produtos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Descricao,Preco,Codigo,Categoria,Ativo,Observacoes")] Produto produto)
        {
            try
            {
                if (id != produto.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    await _crmService.UpdateProdutoAsync(produto);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Erro ao atualizar produto: " + ex.Message);
            }

            return View(produto);
        }

        // GET: Produtos/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var produto = await _crmService.GetProdutoByIdAsync(id);
                if (produto == null)
                {
                    return NotFound();
                }

                return View(produto);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        // POST: Produtos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _crmService.DeleteProdutoAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        // GET: Produtos/Ativos
        public async Task<IActionResult> Ativos()
        {
            try
            {
                var produtos = await _crmService.GetProdutosAtivosAsync();
                return View(produtos);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }
    }
} 