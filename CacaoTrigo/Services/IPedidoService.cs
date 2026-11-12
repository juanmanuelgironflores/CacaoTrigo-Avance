using CacaoTrigo.Models;

namespace CacaoTrigo.Services;

public interface IPedidoService
{
    Task<List<Pedido>> ObtenerTodosAsync();
    Task<Pedido?> ObtenerPorIdAsync(int id);
    Task<bool> CrearAsync(Pedido pedido);
    Task<bool> ActualizarAsync(int id, Pedido pedido);
    Task<bool> EliminarAsync(int id);
}