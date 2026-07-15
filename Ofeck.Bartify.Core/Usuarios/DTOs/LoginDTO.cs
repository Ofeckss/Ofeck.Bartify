namespace Ofeck.Bartify.Core.Usuarios.DTOs;

public readonly record struct LoginDTO
(
    Guid Id,
    string Nombre,
    string? Apellido,
    string Email,
    string Password,
    DateTime? FechaNacimiento,
    string? NumeroCel,
    int Rol,
    bool Activo
);
