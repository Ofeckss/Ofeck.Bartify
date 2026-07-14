namespace Ofeck.Bartify.Core.Articulos.Requests;

public readonly record struct UpdateArticuloRequest
(
    string? Nombre,
    string? Descripcion,
    double? Precio,
    bool? EsTrueque,
    int? CategoriaId,
    int? EstadoId,
    int? UbicacionId
);