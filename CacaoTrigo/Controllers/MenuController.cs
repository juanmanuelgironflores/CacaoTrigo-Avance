using CacaoTrigo.Data;
using CacaoTrigo.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CacaoTrigo.Controllers;

/// <summary>Menú público: cualquiera puede verlo; solo los clientes pueden pedir.</summary>
public class MenuController : Controller
{
    private readonly CacaoTrigoContext _context;

    public MenuController(CacaoTrigoContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(int? categoriaId)
    {
        var categorias = await _context.Categorias
            .OrderBy(c => c.Nombre)
            .ToListAsync();

        var consulta = _context.Productos
            .Include(p => p.Categoria)
            .Where(p => p.Disponible);

        if (categoriaId.HasValue)
        {
            consulta = consulta.Where(p => p.CategoriaId == categoriaId.Value);
        }

        var productos = (await consulta.ToListAsync())
            .OrderBy(p => p.Categoria?.Nombre)
            .ThenBy(p => p.Nombre)
            .ToList();

        return View(new MenuViewModel
        {
            Categorias = categorias,
            Productos = productos,
            CategoriaSeleccionada = categoriaId
        });
    }
}
