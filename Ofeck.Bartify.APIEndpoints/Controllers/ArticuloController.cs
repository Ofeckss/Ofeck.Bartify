using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ofeck.Bartify.Core.Articulos;
using Ofeck.Bartify.Core.Articulos.DTOs;
using Ofeck.Bartify.Core.Articulos.Requests;

namespace Ofeck.Bartify.APIEndpoints.Controllers;

[ApiController]
[Route("api/articulos")]
public class ArticuloController: ControllerBase
{
    private readonly ArticuloService articuloService;
    public ArticuloController(ArticuloService articuloService)
    {
        this.articuloService = articuloService;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateArticulo([FromBody] CreateArticuloRequest request)
    {
        try
        {
            var usuarioId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            
            var articulo = await this.articuloService.Create(request, usuarioId);

            return this.Created($"api/articulos/{articulo.Id}",articulo);
        } catch (Exception e)
        {
            return this.Problem(e.Message, title: e.StackTrace);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var articulos = await this.articuloService.GetAll();

            return this.Ok(articulos);
        } catch (Exception e)
        {
            return this.Problem(e.Message, title: e.StackTrace);
        }
    }

    [HttpGet("usuario/{id:guid}")]
    public async Task<IActionResult> GetByUserId(Guid id)
    {
        try
        {
            var articulos = await this.articuloService.GetByUserId(id);

            return this.Ok(articulos);
        } catch (Exception e)
        {
            return this.Problem(e.Message, title: e.StackTrace);
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var articulo = await this.articuloService.GetById(id);
            
            return this.Ok(articulo);
        } catch (Exception e)
        {
            return this.Problem(e.Message, title: e.StackTrace);
        }
    }
    
    [HttpPost("search")]
    public async Task<IActionResult> GetFiltered(GetFilteredRequest request)
    {
        try
        {
            var result = await this.articuloService.GetFiltered(request);
            
            return this.Ok(result);
        } catch (Exception e)
        {
            return this.Problem(e.Message, title: e.StackTrace);
        }
    }

    [HttpPatch("{id:guid}/update")]
    public async Task<IActionResult> Patch(Guid id, [FromBody] UpdateArticuloRequest request)
    {
        try
        {
            await this.articuloService.Update(request, id);

            return this.Ok();
        } catch (Exception e)
        {
            return this.Problem(e.Message, title: e.StackTrace);
        }
    }

    [HttpDelete("{id:guid}/delete")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await this.articuloService.Delete(id);

            return this.Ok();
        } catch (Exception e)
        {
            return this.Problem(e.Message, title: e.StackTrace);
        }
    }
}