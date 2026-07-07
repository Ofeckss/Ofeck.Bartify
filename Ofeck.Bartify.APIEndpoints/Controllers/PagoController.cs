using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ofeck.Bartify.Core.Pagos;
using Ofeck.Bartify.Core.Pagos.DTOs;

namespace Ofeck.Bartify.APIEndpoints.Controllers;

[ApiController]
[Route("api/pagos")]
public class PagosController : ControllerBase
{
    private readonly PagoService _pagoService;

    public PagosController(PagoService pagoService)
    {
        _pagoService = pagoService;
    }

    [Authorize]
    [HttpPost("checkout")]
    public async Task<IActionResult> CrearCheckout([FromBody] CrearCheckoutRequest request)
    {
        try
        {
            var response = await _pagoService.CrearCheckoutSessionAsync(request.ChatId, request.monto);
            return Ok(response);
        } catch (Exception e)
        {
            return this.Problem(e.StackTrace, title: "Ha ocurrido un error inesperado.");
        }
    } 
    
    [HttpPost("webhook")]
    public async Task<IActionResult> Webhook()
    {
        var json = await new StreamReader(Request.Body).ReadToEndAsync();
        var stripeSignature = Request.Headers["Stripe-Signature"];

        await _pagoService.ProcesarWebhookAsync(json, stripeSignature!);

        return Ok();
    }
}