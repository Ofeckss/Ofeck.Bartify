namespace Ofeck.Bartify.Core.Models;

public readonly record struct Articulo
(
    Guid Id,
    string Nombre,
    string Descripcion,
    double Precio,
    Guid VendedorId,
    int CategoriaId,
    bool EsTrueque,
    int EstadoId,
    int UbicacionId,
    bool Disponible,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);