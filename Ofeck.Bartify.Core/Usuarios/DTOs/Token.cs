namespace Ofeck.Bartify.Core.Usuarios.DTOs;

public readonly record struct Token
(
    Guid Id,
    string Email
);