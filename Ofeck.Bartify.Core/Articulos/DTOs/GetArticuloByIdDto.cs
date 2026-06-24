namespace Ofeck.Bartify.Core.Articulos.DTOs;

public readonly record struct GetArticuloByIdDto
(
    Guid Id,
    string Nombre,
    string Descripcion,
    double Precio,
    bool EsTrueque,
    bool Disponible,
    UbicacionArticulo Ubicacion,
    DateTime CreatedAt,
    CategoriaArticulo Categoria,
    Vendedor Vendedor
); 