namespace Ofeck.Bartify.Core.Articulos.DTOs;

public readonly record struct GetArticuloDto(
    Guid Id,
    string Nombre,
    string Descripcion,
    double Precio,
    bool EsTrueque,
    bool Disponible,
    string Url,
    UbicacionArticulo Ubicacion,
    DateTime CreatedAt,
    CategoriaArticulo Categoria,
    Vendedor Vendedor
);

public readonly record struct CategoriaArticulo(int Id, string Nombre);
public readonly record struct Vendedor(Guid VendedorId, string Nombre, string Apellido);
public readonly record struct UbicacionArticulo(int Id, string Nombre);