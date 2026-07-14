namespace Ofeck.Bartify.Core.Usuarios.DTOs;

public readonly record struct LoginDTO
(
    Guid Id,
    string Nombre,
    string Email,
    string Password,
    int Rol,
    bool Activo
);
