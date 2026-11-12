using CacaoTrigo.Models;

namespace CacaoTrigo.Repositories;

public interface IReservaRepository
{
    Task<List<Reserva>> ObtenerTodosAsync();
    Task<Reserva?> ObtenerPorIdAsync(int id);
    Task<bool> MesaExisteAsync(int mesaId);
    Task AgregarAsync(Reserva reserva);
    Task GuardarCambiosAsync();
    Task EliminarAsync(Reserva reserva);
}