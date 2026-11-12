using CacaoTrigo.Models;
using CacaoTrigo.Services;
using Microsoft.AspNetCore.Mvc;

public class PedidosController : Controller
{
    private readonly IPedidoService _pedidoService;

    public PedidosController(IPedidoService pedidoService)
    {
        _pedidoService = pedidoService;
    }

    public async Task<IActionResult> Index()
    {
        var pedidos = await _pedidoService.ObtenerTodosAsync();
        return View(pedidos);
    }

    public async Task<IActionResult> Details(int id)
    {
        var pedido = await _pedidoService.ObtenerPorIdAsync(id);

        if (pedido == null)
        {
            return NotFound();
        }

        return View(pedido);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Fecha,Estado,NombreContacto,TelefonoContacto,DireccionEntrega")]
        Pedido pedido)
    {
        pedido.Total = 0m;

        if (pedido.Fecha == default)
        {
            pedido.Fecha = DateTime.Now;
        }

        if (!ModelState.IsValid)
        {
            return View(pedido);
        }

        var creado = await _pedidoService.CrearAsync(pedido);

        if (!creado)
        {
            ModelState.AddModelError(string.Empty, "No se pudo guardar el pedido.");
            return View(pedido);
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var pedido = await _pedidoService.ObtenerPorIdAsync(id);

        if (pedido == null)
        {
            return NotFound();
        }

        return View(pedido);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        [Bind("PedidoId,Fecha,Estado,NombreContacto,TelefonoContacto,DireccionEntrega")]
        Pedido pedido)
    {
        var pedidoGuardado = await _pedidoService.ObtenerPorIdAsync(pedido.PedidoId);

        if (pedidoGuardado == null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            pedido.Total = pedidoGuardado.Total;
            return View(pedido);
        }

        pedido.Total = pedidoGuardado.Total;

        var actualizado = await _pedidoService.ActualizarAsync(
            pedido.PedidoId,
            pedido);

        if (!actualizado)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var pedido = await _pedidoService.ObtenerPorIdAsync(id);

        if (pedido == null)
        {
            return NotFound();
        }

        return View(pedido);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(
        [FromForm(Name = "PedidoId")] int id)
    {
        var eliminado = await _pedidoService.EliminarAsync(id);

        if (!eliminado)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Index));
    }
}