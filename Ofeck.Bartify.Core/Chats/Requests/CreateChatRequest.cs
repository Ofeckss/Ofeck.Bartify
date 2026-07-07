namespace Ofeck.Bartify.Core.Chats.Requests;

public readonly record struct CreateChatRequest
(
    Guid Articulo,
    Guid Vendedor,
    string Nombre,
    bool EsTrueque
);