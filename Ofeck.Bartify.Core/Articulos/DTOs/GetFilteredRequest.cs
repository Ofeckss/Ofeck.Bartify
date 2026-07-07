namespace Ofeck.Bartify.Core.Articulos.DTOs;

public readonly record struct GetFilteredRequest
(
    string? Nombre,
    double? PrecioMin,
    double? PrecioMax,
    int? CategoriaId,
    bool? EsTrueque,
    int? UbicacionId,
    int? EstadoId
);