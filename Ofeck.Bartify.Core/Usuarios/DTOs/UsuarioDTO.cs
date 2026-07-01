namespace Ofeck.Bartify.Core.Usuarios.DTOs;

public readonly record struct UsuarioDTO
(
    Guid Id,
    string Nombre,
    string? Apellido,
    DateOnly? FechaNacimiento,
    string? NumeroCel,
    double? Rating,
    bool Activo
);