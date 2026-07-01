namespace Ofeck.Bartify.Core.Transacciones.Requests;

public readonly record struct ConfirmarRequest
(
    Guid ChatId,
    List<Guid> Articulos,
    double? Precio
);