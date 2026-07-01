using Ofeck.Bartify.Core.Usuarios.DTOs;
using Ofeck.Bartify.Core.Articulos.DTOs;

namespace Ofeck.Bartify.Core.Transacciones.Responses;

public readonly record struct GetTransaccionResponse
(
    Guid Id,
    Guid ChatId,
    bool EsTrueque,
    double PrecioFinal,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    List<MiniArticuloDTO> ArticulosVendedor,
    List<MiniArticuloDTO> ArticulosComprador
);