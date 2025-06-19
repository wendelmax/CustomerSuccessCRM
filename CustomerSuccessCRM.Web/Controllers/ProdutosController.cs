using Microsoft.AspNetCore.Mvc;
using CustomerSuccessCRM.Lib.Services;
using CustomerSuccessCRM.Lib.Models;
using CustomerSuccessCRM.Web.ViewModels;

namespace CustomerSuccessCRM.Web.Controllers
{
    public class ProdutosController : Controller
    {
        private readonly ProdutoService _produtoService;

        public ProdutosController(ProdutoService produtoService)
        {
            _produtoService = produtoService;
        }

        // GET: Produtos
        public async Task<IActionResult> Index(CategoriaProduto? categoria, string searchTerm)
        {
            try
            {
                List<Produto> produtos;

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    // Buscar todos e filtrar por nome
                    produtos = await _produtoService.ListarTodosAsync();
                    produtos = produtos.Where(p => p.Nome.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                                  (p.Descricao != null && p.Descricao.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))).ToList();
                }
                else if (categoria.HasValue)
                {
                    produtos = await _produtoService.BuscarPorCategoriaAsync(categoria.Value);
                }
                else
                {
                    produtos = await _produtoService.ListarTodosAsync();
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
                var produto = await _produtoService.BuscarPorIdAsync(id);
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
        public async Task<IActionResult> Create([Bind("Nome,Descricao,PrecoBase,Categoria,QuantidadeEstoque")] Produto produto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _produtoService.CadastrarAsync(produto);
                    TempData["Success"] = "Produto criado com sucesso!";
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
                var produto = await _produtoService.BuscarPorIdAsync(id);
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Descricao,PrecoBase,Categoria,QuantidadeEstoque,Ativo")] Produto produto)
        {
            try
            {
                if (id != produto.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    await _produtoService.AtualizarAsync(produto);
                    TempData["Success"] = "Produto atualizado com sucesso!";
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
                var produto = await _produtoService.BuscarPorIdAsync(id);
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
                await _produtoService.DeletarAsync(id);
                TempData["Success"] = "Produto excluído com sucesso!";
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
                var produtos = await _produtoService.ListarTodosAsync();
                var produtosAtivos = produtos.Where(p => p.Ativo).ToList();
                return View(produtosAtivos);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        // GET: Produtos/MaisVendidos
        public async Task<IActionResult> MaisVendidos()
        {
            try
            {
                var produtos = await _produtoService.ListarMaisVendidosAsync(10);
                return View(produtos);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }
    }
} 