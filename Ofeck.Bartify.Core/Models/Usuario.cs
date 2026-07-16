namespace Ofeck.Bartify.Core.Models;

public readonly record struct Usuario 
(
    Guid Id,
    string Nombre,
    string? Apellido,
    DateOnly? FechaNacimiento,
    string Email,
    string? NumeroCel,
    double? Rating,
    string Password,
    bool Activo,
    DateTime FechaCreacion,
    int Rol
);
