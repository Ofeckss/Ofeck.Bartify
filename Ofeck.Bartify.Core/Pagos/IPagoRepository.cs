using Ofeck.Bartify.Core.Models;

namespace Ofeck.Bartify.Core.Pagos;

public interface IPagoRepository
{
    Task InsertarAsync(Pago pago);
    Task<Pago> ObtenerPorSessionIdAsync(string stripeSessionId);
    Task ActualizarEstadoAsync(string stripeSessionId, string nuevoEstado);
}