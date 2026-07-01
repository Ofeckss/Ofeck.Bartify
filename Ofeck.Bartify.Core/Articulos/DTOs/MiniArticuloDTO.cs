namespace Ofeck.Bartify.Core.Articulos.DTOs;

public readonly record struct MiniArticuloDTO
(
    Guid Id,
    string Nombre,
    string Url
);