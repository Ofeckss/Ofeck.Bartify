using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ofeck.Bartify.Core.Transacciones;
using Ofeck.Bartify.Core.Transacciones.Requests;
namespace Ofeck.Bartify.APIEndpoints.Controllers;

[ApiController]
[Route("api/transacciones")]
public class TransaccionController : ControllerBase
{
    private readonly TransaccionService transaccionService;

    public TransaccionController(TransaccionService transaccionService)
    {
        this.transaccionService = transaccionService;
    }

    [HttpPost]
    public async Task<ActionResult> CreateTransaccion([FromBody] CreateTransaccionRequest request)
    {
        try
        {
            await this.transaccionService.CreateTrans(request);
            return this.Created();
        } catch (ArgumentException ae)
        {
            return this.UnprocessableEntity(ae.Message);
        }catch (Exception e)
        {
            return this.Problem(e.Message, title: e.StackTrace);
        } 
    }

    [Authorize]
    [HttpPost("detalles")]
    public async Task<ActionResult> CreateDetalle([FromBody] CreateDetalleRequest request)
    {
        try
        {
            var usuarioId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            
            await this.transaccionService.CreateDetalle(request, usuarioId);
            return this.Created();
        } catch (Exception e)
        {
            return this.Problem(e.Message, title: e.StackTrace);
        } 
    }

    [Authorize]
    [HttpPost("confirmar")]
    public async Task<ActionResult> Confirmar([FromBody] ConfirmarRequest request)
    {
        try
        {
            var usuarioId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            await this.transaccionService.Confirmar(request, usuarioId);
            return this.Ok();
        } catch (Exception e)
        {
            return this.Problem(e.Message, title: e.StackTrace);
        } 
    }

    [Authorize]
    [HttpPost("cancelar")]
    public async Task<ActionResult> Cancelar([FromQuery] Guid chatId)
    {
        try
        {
            var usuarioId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            await this.transaccionService.Cancelar(chatId, usuarioId);
            return this.Ok();
        } catch (Exception e)
        {
            return this.Problem(e.Message,title: e.StackTrace);
        }
    }

    [HttpGet("status/{chatId:guid}")]
    public async Task<ActionResult> GetStatus(Guid chatId)
    {
        try
        {
            var status = await this.transaccionService.GetStatus(chatId);

            return this.Ok(status);
        } catch (Exception e)
        {
            return this.Problem(e.Message, title: e.StackTrace);
        }
    }

    [Authorize]
    [HttpGet("usuario")]
    public async Task<ActionResult> GetByUser()
    {
        try
        {
            var usuarioId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var lista = await this.transaccionService.GetAllByUser(usuarioId);
            return Ok(lista);
        } catch (Exception e)
        {
            return this.Problem(e.Message, title: e.StackTrace);
        }
    }

    [HttpGet("{chatId:guid}")]
    public async Task<ActionResult> GetById([FromRoute] Guid chatId)
    {
        try
        {
            var trans = await this.transaccionService.GetById(chatId);
            
            return Ok(trans);
        } catch (Exception e)
        {
            return this.Problem(e.Message, title: e.StackTrace);
        }
    }

    [HttpGet("detalles/{chatId:guid}")]
    public async Task<ActionResult> GetDetalles([FromRoute] Guid chatId)
    {
        try
        {
            var detalles = await this.transaccionService.GetDetalle(chatId);
            
            return this.Ok(detalles);
        } catch (Exception e)
        {
            return this.Problem(e.Message, title: e.StackTrace);
        }
    }
}
