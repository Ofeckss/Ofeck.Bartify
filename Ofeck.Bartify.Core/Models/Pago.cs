namespace Ofeck.Bartify.Core.Models;

public readonly record struct Pago
(
    Guid Id,
    Guid TransaccionId,
    string StripeSessionId,
    decimal Monto, 
    string Moneda,
    string Estado,
    DateTime FechaCreacion,
    DateTime? FechaActualizacion
);