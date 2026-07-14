namespace Ofeck.Bartify.Core.Usuarios.DTOs;

public readonly record struct TokenRol
    (
        Guid Id,
        string Nombre,
        string Email,
        int Rol
    );
