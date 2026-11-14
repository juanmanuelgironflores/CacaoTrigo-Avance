using CacaoTrigo.Models;
using CacaoTrigo.Repositories;

namespace CacaoTrigo.Services;

public class PedidoService : IPedidoService
{
    private readonly IPedidoRepository _pedidoRepository;

    public PedidoService(IPedidoRepository pedidoRepository)
    {
        _pedidoRepository = pedidoRepository;
    }

    public Task<List<Pedido>> ObtenerTodosAsync()
    {
        return _pedidoRepository.ObtenerTodosAsync();
    }

    public Task<Pedido?> ObtenerPorIdAsync(int id)
    {
        return _pedidoRepository.ObtenerPorIdAsync(id);
    }

    public async Task<bool> CrearAsync(Pedido pedido)
    {
        if (pedido.Fecha == default ||
            string.IsNullOrWhiteSpace(pedido.Estado))
        {
            return false;
        }

        // El total se calcula al agregar productos al pedido.
        pedido.Total = 0m;

        await _pedidoRepository.AgregarAsync(pedido);
        await _pedidoRepository.GuardarCambiosAsync();

        return true;
    }

    public async Task<bool> ActualizarAsync(int id, Pedido pedido)
    {
        if (pedido.Fecha == default ||
            string.IsNullOrWhiteSpace(pedido.Estado))
        {
            return false;
        }

        var pedidoGuardado = await _pedidoRepository.ObtenerPorIdAsync(id);

        if (pedidoGuardado == null)
        {
            return false;
        }

        pedidoGuardado.Fecha = pedido.Fecha;
        pedidoGuardado.Estado = pedido.Estado;
        pedidoGuardado.NombreContacto = pedido.NombreContacto;
        pedidoGuardado.TelefonoContacto = pedido.TelefonoContacto;
        pedidoGuardado.DireccionEntrega = pedido.DireccionEntrega;

        // No cambiar Total aquí: se actualiza con los detalles del pedido.
        await _pedidoRepository.GuardarCambiosAsync();

        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var pedido = await _pedidoRepository.ObtenerPorIdAsync(id);

        if (pedido == null)
        {
            return false;
        }

        await _pedidoRepository.EliminarAsync(pedido);
        await _pedidoRepository.GuardarCambiosAsync();

        return true;
    }
}