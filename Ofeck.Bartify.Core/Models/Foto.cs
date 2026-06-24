namespace Ofeck.Bartify.Core.Models;

public readonly record struct Foto
(
    Guid Id,
    string Url,
    Guid ArticuloId,
    int orden
);