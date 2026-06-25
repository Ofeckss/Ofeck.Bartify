namespace Ofeck.Bartify.Core.Chats.DTOs;

public readonly record struct ChatDTO
(
    Guid Id,
    Guid ArticuloPrincipal,
    string NombreArticulo,
    string UrlArticulo,
    string Url
);