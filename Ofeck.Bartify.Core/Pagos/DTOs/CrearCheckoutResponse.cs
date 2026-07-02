namespace Ofeck.Bartify.Core.Pagos.DTOs;

public readonly record struct CrearCheckoutResponse
(
    string CheckoutUrl,
    string SessionId
);