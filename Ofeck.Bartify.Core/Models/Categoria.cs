namespace Ofeck.Bartify.Core.Models;

public readonly record struct Categoria
(
    int Id,
    string Nombre,
    int PadreId
);