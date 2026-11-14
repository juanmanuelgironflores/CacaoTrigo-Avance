using CacaoTrigo.Data;
using CacaoTrigo.Models;
using CacaoTrigo.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CacaoTrigo.Controllers;

/// <summary>Historial de compras del cliente que inició sesión.</summary>
[Authorize(Roles = RolesApp.Cliente)]
public class MisPedidosController : Controller
{
    private readonly CacaoTrigoContext _context;
    private readonly UserManager<ApplicationUser> _usuarios;

    public MisPedidosController(
        CacaoTrigoContext context,
        UserManager<ApplicationUser> usuarios)
    {
        _context = context;
        _usuarios = usuarios;
    }

    public async Task<IActionResult> Index()
    {
        var usuarioId = _usuarios.GetUserId(User);

        var pedidos = await _context.Pedidos
            .Where(p => p.UsuarioId == usuarioId)
            .OrderByDescending(p => p.Fecha)
            .ToListAsync();

        var ids = pedidos.Select(p => p.PedidoId).ToList();

        var detalles = await _context.DetallesPedido
            .Include(d => d.Producto)
            .Where(d => ids.Contains(d.PedidoId))
            .ToListAsync();

        var historial = pedidos
            .Select(p => new PedidoHistorialViewModel
            {
                Pedido = p,
                Detalles = detalles.Where(d => d.PedidoId == p.PedidoId).ToList()
            })
            .ToList();

        return View(historial);
    }
}
