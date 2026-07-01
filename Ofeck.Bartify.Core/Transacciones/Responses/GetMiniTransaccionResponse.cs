namespace Ofeck.Bartify.Core.Transacciones.Responses;

public readonly record struct GetMiniTransaccionResponse
(
    Guid Id,
    Guid ChatId,
    bool EsTrueque,
    double PrecioFinal,
    DateTime CreatedAt,
    DateTime UpdatedAt
);