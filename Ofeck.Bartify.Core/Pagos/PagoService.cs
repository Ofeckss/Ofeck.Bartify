using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.Checkout;
using Ofeck.Bartify.Core.Transacciones;
using Ofeck.Bartify.Core.Models;
using Ofeck.Bartify.Core.Pagos.DTOs;

namespace Ofeck.Bartify.Core.Pagos;

public class PagoService
{
    private readonly IPagoRepository _pagoRepository;
    private readonly ITransaccionRepository _transaccionRepository;
    private readonly IConfiguration _configuration;

    public PagoService(
        IPagoRepository pagoRepository,
        ITransaccionRepository transaccionRepository,
        IConfiguration configuration)
    {
        _pagoRepository = pagoRepository;
        _transaccionRepository = transaccionRepository;
        _configuration = configuration;
    }

    public async Task<CrearCheckoutResponse> CrearCheckoutSessionAsync(Guid chatId)
    {
        var transaccionId = await _transaccionRepository.GetTransId(chatId);
        var transaccion = await _transaccionRepository.GetSemiById(transaccionId);

        if (transaccion.EsTrueque)
            throw new InvalidOperationException("Esta transacción es un trueque, no requiere pago.");

        if (transaccion.PrecioFinal <= 0)
            throw new InvalidOperationException("La transacción no tiene un precio válido para procesar el pago.");

        var monto = (decimal)transaccion.PrecioFinal;

        var frontendUrl = _configuration["Frontend:BaseUrl"];

        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string> { "card" },
            LineItems = new List<SessionLineItemOptions>
            {
                new()
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "mxn",
                        UnitAmount = (long)(monto * 100), // Stripe usa centavos
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = $"Transacción Bartify #{transaccionId}"
                        }
                    },
                    Quantity = 1
                }
            },
            Mode = "payment",
            SuccessUrl = $"{frontendUrl}/chats/{chatId}?pago=exitoso",
            CancelUrl = $"{frontendUrl}/chats/{chatId}?pago=cancelado",
            Metadata = new Dictionary<string, string>
            {
                { "transaccion_id", transaccionId.ToString() },
                { "chat_id", chatId.ToString() }
            }
        };

        var service = new SessionService();
        Session session = await service.CreateAsync(options);

        var pago = new Pago
        {
            Id = Guid.CreateVersion7(),
            TransaccionId = transaccionId,
            StripeSessionId = session.Id,
            Monto = monto,
            Moneda = "MXN",
            Estado = "pendiente",
            FechaCreacion = DateTime.UtcNow
        };

        await _pagoRepository.InsertarAsync(pago);

        return new CrearCheckoutResponse
        {
            CheckoutUrl = session.Url,
            SessionId = session.Id
        };
    }

    public async Task ProcesarWebhookAsync(string json, string stripeSignature)
    {
        var webhookSecret = _configuration["Stripe:WebhookSecret"];

        var stripeEvent = EventUtility.ConstructEvent(json, stripeSignature, webhookSecret);

        if (stripeEvent.Type == EventTypes.CheckoutSessionCompleted)
        {
            var session = stripeEvent.Data.Object as Session;
            if (session is not null)
            {
                await _pagoRepository.ActualizarEstadoAsync(session.Id, "pagado");
                // Pendiente: aquí conectas ConfirmarComprador si decides automatizarlo
            }
        }
        else if (stripeEvent.Type == EventTypes.CheckoutSessionExpired)
        {
            var session = stripeEvent.Data.Object as Session;
            if (session is not null)
            {
                await _pagoRepository.ActualizarEstadoAsync(session.Id, "expirado");
            }
        }
    }
}