using ControleEstoque.Data;
using ControleEstoque.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ControleEstoque.Controllers
{
    [Authorize]
    public class MovimentacaosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MovimentacaosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Movimentacaos
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Movimentacao.Include(m => m.Produto).Include(m => m.Usuario);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Movimentacaos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movimentacao = await _context.Movimentacao
                .Include(m => m.Produto)
                .Include(m => m.Usuario)
                .FirstOrDefaultAsync(m => m.MovimentacaoId == id);
            if (movimentacao == null)
            {
                return NotFound();
            }

            return View(movimentacao);
        }

        // GET: Movimentacaos/Create
        public IActionResult Create()
        {
            ViewData["ProdutoId"] = new SelectList(_context.Produto, "ProdutoId", "Nome");
            ViewData["UsuarioId"] = new SelectList(_context.Users, "Id", "Email");
            return View();
        }

        // POST: Movimentacaos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MovimentacaoId,ProdutoId,Quantidade,Tipo,DataMovimentacao,UsuarioId,Observacao")] Movimentacao movimentacao)
        {
            if (ModelState.IsValid)
            {
                //As alterações da lógica, incluidas a partir desse ponto so serao executadas, se os dados vierem corretos do formulario;

                movimentacao.DataMovimentacao = DateTime.Now;

                //localizar um registro por id

                var produto = _context.Produto.FirstOrDefault(p => p.ProdutoId == movimentacao.ProdutoId);

                // verificar o tipo de movimentação
                // se for entrada, entao eu irei aumentar a quantidade
                // se nao -> diminuir a quantidade (diminuir no estoque)

                if (movimentacao.Tipo == "Entrada")
                {
                    //produto.EstoqueAtual = produto.EstoqueAtual + movimentacao.Quantidade; - faz a mesma coisa que a linha abaixo.
                    produto.EstoqueAtual += movimentacao.Quantidade;
                }
                else
                {
                    // Antes de subtrair do estoque, é preciso verificar se o estoque atual é igual ou maior que a quantidade
                    if (produto.EstoqueAtual >= movimentacao.Quantidade)
                    {
                        produto.EstoqueAtual -= movimentacao.Quantidade;
                    }
                    else
                    {
                        ViewData["Alerta"] = "O Estoque Atual Do Produto " + produto.Nome + " Está Insuficiente.";
                        ViewData["ProdutoId"] = new SelectList(_context.Produto, "ProdutoId", "Nome", movimentacao.ProdutoId);
                        ViewData["UsuarioId"] = new SelectList(_context.Users, "Id", "Email", movimentacao.UsuarioId);
                        return View(movimentacao);
                    }
                }

                _context.Add(movimentacao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProdutoId"] = new SelectList(_context.Produto, "ProdutoId", "Nome", movimentacao.ProdutoId);
            ViewData["UsuarioId"] = new SelectList(_context.Users, "Id", "Email", movimentacao.UsuarioId);
            return View(movimentacao);
        }

        // GET: Movimentacaos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movimentacao = await _context.Movimentacao.FindAsync(id);
            if (movimentacao == null)
            {
                return NotFound();
            }
            ViewData["ProdutoId"] = new SelectList(_context.Produto, "ProdutoId", "Marca", movimentacao.ProdutoId);
            ViewData["UsuarioId"] = new SelectList(_context.Users, "Id", "Email", movimentacao.UsuarioId);
            return View(movimentacao);
        }

        // POST: Movimentacaos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MovimentacaoId,ProdutoId,Quantidade,Tipo,DataMovimentacao,UsuarioId,Observacao")] Movimentacao movimentacao)
        {
            if (id != movimentacao.MovimentacaoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movimentacao);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovimentacaoExists(movimentacao.MovimentacaoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProdutoId"] = new SelectList(_context.Produto, "ProdutoId", "Marca", movimentacao.ProdutoId);
            ViewData["UsuarioId"] = new SelectList(_context.Users, "Id", "Email", movimentacao.UsuarioId);
            return View(movimentacao);
        }

        // GET: Movimentacaos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movimentacao = await _context.Movimentacao
                .Include(m => m.Produto)
                .Include(m => m.Usuario)
                .FirstOrDefaultAsync(m => m.MovimentacaoId == id);
            if (movimentacao == null)
            {
                return NotFound();
            }

            return View(movimentacao);
        }

        // POST: Movimentacaos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movimentacao = await _context.Movimentacao.FindAsync(id);
            if (movimentacao != null)
            {
                //verificar o tipo da movimentação
                // se for entrada -> retirar a quantidade do estoque atual
                // senao -> devolver a quantidade o estoque atual

                // buscar o produto no estoque a partir do ProdutoId da movimentação;
                var produto = _context.Produto.FirstOrDefault(p => p.ProdutoId == movimentacao.ProdutoId);

                if (movimentacao.Tipo == "Entrada")
                {
                    produto.EstoqueAtual -= movimentacao.Quantidade;
                }
                else
                {
                    produto.EstoqueAtual += movimentacao.Quantidade;
                }
                _context.Movimentacao.Remove(movimentacao);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovimentacaoExists(int id)
        {
            return _context.Movimentacao.Any(e => e.MovimentacaoId == id);
        }
    }
}
