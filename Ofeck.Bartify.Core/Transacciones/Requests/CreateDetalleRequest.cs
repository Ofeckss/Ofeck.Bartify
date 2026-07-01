namespace Ofeck.Bartify.Core.Transacciones.Requests;

public readonly record struct CreateDetalleRequest
(
    Guid ChatId,
    Guid ArticuloId
);