using CacaoTrigo.Data;
using CacaoTrigo.Models;
using CacaoTrigo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class ProductosController : Controller
{
    private readonly IProductoService _productoService;
    private readonly CacaoTrigoContext _context;

    public ProductosController(
        IProductoService productoService,
        CacaoTrigoContext context)
    {
        _productoService = productoService;
        _context = context;
    }

    // GET: Productos
    public async Task<IActionResult> Index(
        string? buscar,
        int? categoriaId,
        string? ordenar)
    {
        var productos = await _productoService.BuscarYOrdenarAsync(
            buscar,
            categoriaId,
            ordenar);

        var categorias = await _context.Categorias
            .OrderBy(c => c.Nombre)
            .ToListAsync();

        ViewBag.Categorias = new SelectList(
            categorias,
            "CategoriaId",
            "Nombre",
            categoriaId);

        ViewBag.BuscarActual = buscar;
        ViewBag.CategoriaActual = categoriaId;
        ViewBag.OrdenActual = ordenar;

        return View(productos);
    }

    // GET: Productos/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var producto = await _productoService.ObtenerPorIdAsync(id.Value);

        if (producto == null)
        {
            return NotFound();
        }

        return View(producto);
    }

    // GET: Productos/Create
    public async Task<IActionResult> Create()
    {
        await CargarCategoriasAsync();
        return View();
    }

    // POST: Productos/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Nombre,Descripcion,Precio,ImagenUrl,Disponible,CategoriaId")]
        Producto producto)
    {
        if (!await _context.Categorias.AnyAsync(
                c => c.CategoriaId == producto.CategoriaId))
        {
            ModelState.AddModelError(
                nameof(producto.CategoriaId),
                "Selecciona una categoría válida.");
        }

        if (ModelState.IsValid)
        {
            var creado = await _productoService.CrearAsync(producto);

            if (creado)
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(
                string.Empty,
                "No se pudo guardar el producto.");
        }

        await CargarCategoriasAsync(producto.CategoriaId);
        return View(producto);
    }

    // GET: Productos/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var producto = await _productoService.ObtenerPorIdAsync(id.Value);

        if (producto == null)
        {
            return NotFound();
        }

        await CargarCategoriasAsync(producto.CategoriaId);
        return View(producto);
    }

    // POST: Productos/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int? id,
        [Bind("ProductoId,Nombre,Descripcion,Precio,ImagenUrl,Disponible,CategoriaId")]
        Producto producto)
    {
        if (id == null || id != producto.ProductoId)
        {
            return NotFound();
        }

        if (!await _context.Categorias.AnyAsync(
                c => c.CategoriaId == producto.CategoriaId))
        {
            ModelState.AddModelError(
                nameof(producto.CategoriaId),
                "Selecciona una categoría válida.");
        }

        if (ModelState.IsValid)
        {
            var actualizado = await _productoService.ActualizarAsync(
                producto.ProductoId,
                producto);

            if (actualizado)
            {
                return RedirectToAction(nameof(Index));
            }

            return NotFound();
        }

        await CargarCategoriasAsync(producto.CategoriaId);
        return View(producto);
    }

    // GET: Productos/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var producto = await _productoService.ObtenerPorIdAsync(id.Value);

        if (producto == null)
        {
            return NotFound();
        }

        return View(producto);
    }

    // POST: Productos/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int productoId)
    {
        var eliminado = await _productoService.EliminarAsync(productoId);

        if (!eliminado)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task CargarCategoriasAsync(
        int? categoriaSeleccionada = null)
    {
        var categorias = await _context.Categorias
            .OrderBy(c => c.Nombre)
            .ToListAsync();

        ViewBag.CategoriaId = new SelectList(
            categorias,
            "CategoriaId",
            "Nombre",
            categoriaSeleccionada);
    }
}