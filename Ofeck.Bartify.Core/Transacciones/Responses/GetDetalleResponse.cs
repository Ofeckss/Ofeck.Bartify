namespace Ofeck.Bartify.Core.Transacciones.Responses;
using Ofeck.Bartify.Core.Articulos.DTOs;

public readonly record struct GetDetalleResponse
(
    Guid Id,
    bool OfrecidoVendedor,
    MiniArticuloDTO Articulo
);