namespace Ofeck.Bartify.Core.Transacciones.Responses;

public readonly record struct GetStatusResponse
(
    bool Confirmado,
    bool Terminado
);