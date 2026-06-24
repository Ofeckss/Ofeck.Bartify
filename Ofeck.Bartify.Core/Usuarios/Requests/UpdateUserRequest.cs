namespace Ofeck.Bartify.Core.Usuarios.Requests;

public readonly record struct UpdateUserRequest
(
    string? Nombre,
    string? Apellido,
    DateOnly? FechaNacimiento,
    string? NumeroCel
);