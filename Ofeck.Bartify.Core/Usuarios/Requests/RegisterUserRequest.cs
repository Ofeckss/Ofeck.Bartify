namespace Ofeck.Bartify.Core.Usuarios.Requests;

public readonly record struct RegisterUserRequest
(
    string Nombre,
    string Email,
    string Password
    // DateOnly FechaNacimiento
);