using CacaoTrigo.Models;

namespace CacaoTrigo.Repositories;

public interface IPedidoRepository
{
    Task<List<Pedido>> ObtenerTodosAsync();
    Task<Pedido?> ObtenerPorIdAsync(int id);
    Task AgregarAsync(Pedido pedido);
    Task GuardarCambiosAsync();
    Task EliminarAsync(Pedido pedido);
}