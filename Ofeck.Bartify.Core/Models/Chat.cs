namespace Ofeck.Bartify.Core.Models;

public readonly record struct Chat
(
    Guid Id,
    Guid ArticuloPrincipal,
    Guid Comprador,
    Guid Vendedor,
    string Url,
    DateTime CreatedAt
);