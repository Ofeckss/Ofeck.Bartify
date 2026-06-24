namespace Ofeck.Bartify.Core.Usuarios.DTOs;

public readonly record struct LoginDTO
(
    Guid Id,
    string Email,
    string Password,
    bool Activo
);