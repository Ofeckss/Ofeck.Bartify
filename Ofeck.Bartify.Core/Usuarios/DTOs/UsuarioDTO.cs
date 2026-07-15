namespace Ofeck.Bartify.Core.Usuarios.DTOs;

public readonly record struct UsuarioDTO
(
    Guid Id,
    string Nombre,
    string? Apellido,
    DateTime? FechaNacimiento,
    string? NumeroCel,
    double? Rating,
    bool Activo,
    int Rol
);