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

    public async Task<CrearCheckoutResponse> CrearCheckoutSessionAsync(Guid chatId, decimal monto)
    {
        var transaccionId = await _transaccionRepository.GetTransId(chatId);
        
        await this._pagoRepository.ActualizarPrecio(transaccionId, monto);

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
                        UnitAmount = (long)(monto * 100),
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

                if (session.Metadata.TryGetValue("chat_id", out var chatIdStr)
                    && Guid.TryParse(chatIdStr, out var chatId))
                {
                    double? monto = session.AmountTotal.HasValue
                        ? session.AmountTotal.Value / 100.0
                        : null;

                    await this.ConfirmarCompradorAutomatico(chatId, monto);
                }
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
    
    public async Task ConfirmarCompradorAutomatico(Guid chatId, double? precio)
    {
        await this._transaccionRepository.ConfirmarComprador(chatId, precio);
    }
}