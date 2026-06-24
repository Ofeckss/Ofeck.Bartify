using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Ofeck.Bartify.Core.Models;
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
        }catch (Exception e)
        {
            return this.Problem(e.Message, title: e.StackTrace);
        }
    }
}