namespace Ofeck.Bartify.Core.Pagos.DTOs;

public readonly record struct CrearCheckoutRequest
(
    Guid ChatId,
    decimal monto
);