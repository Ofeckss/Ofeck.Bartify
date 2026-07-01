namespace Ofeck.Bartify.Core.Transacciones.Requests;

public readonly record struct CreateTransaccionRequest
(
    Guid ChatId,
    bool EsTrueque
);