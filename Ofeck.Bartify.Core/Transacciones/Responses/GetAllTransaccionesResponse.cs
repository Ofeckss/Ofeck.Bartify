using Ofeck.Bartify.Core.Usuarios.DTOs;

namespace Ofeck.Bartify.Core.Transacciones.Responses;

public readonly record struct GetAllTransaccionesResponse
(
    Guid Id,
    bool EsTrueque,
    double PrecioFinal,
    DateTime CreatedAt,
    string NombreMainArticulo,
    string UrlMainArticulo,
    UsuarioDTO OtroUsuario
);