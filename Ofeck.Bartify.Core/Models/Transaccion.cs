namespace Ofeck.Bartify.Core.Models;

public readonly record struct Transaccion
(
    Guid Id,
    Guid ChatId,
    bool EsTrueque,
    double? PrecioFinal,
    bool ConfirmadoComprador,
    bool ConfirmadoVendedor,
    DateTime CreatedAt,
    DateTime UpdatedAt
);