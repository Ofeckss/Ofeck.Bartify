namespace Ofeck.Bartify.Core.Articulos.Requests;

public readonly record struct CreateArticuloRequest
(
    string Nombre,
    string Descripcion,
    double Precio,
    int CategoriaId,
    bool EsTrueque,
    int EstadoId,
    int UbicacionId
);