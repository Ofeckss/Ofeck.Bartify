namespace Ofeck.Bartify.Core.Models;

public readonly record struct DetalleTransaccion
(
    Guid Id,
    Guid TransaccionId,
    Guid ArticuloId,
    bool OfrecidoVendedor,
    bool Final
);