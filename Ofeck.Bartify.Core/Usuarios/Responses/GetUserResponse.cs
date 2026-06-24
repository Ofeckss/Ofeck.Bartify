namespace Ofeck.Bartify.Core.Usuarios.Responses;

public readonly record struct GetUserResponse
(
    string Nombre,
    string Apellido,
    DateOnly FechaNacimiento,
    double? Rating,
    bool Activo
);