namespace Ofeck.Bartify.Core.Transacciones.Responses;

public readonly record struct Confirmador
(
    bool ConfirmadoV,
    bool ConfirmadoC,
    bool Terminado
);