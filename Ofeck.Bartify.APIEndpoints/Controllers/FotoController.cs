using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Ofeck.Bartify.APIEndpoints.Auth;
using Ofeck.Bartify.Core.Fotos;
using Ofeck.Bartify.Core.Models;
using Ofeck.Bartify.Core.Fotos.Requests;

namespace Ofeck.Bartify.APIEndpoints.Controllers;

[ApiController]
[Route("api/fotos")]
public class FotoController: ControllerBase
{
    private readonly FotoService fotoService;
    public FotoController(FotoService fotoService)
    {
        this.fotoService = fotoService;
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromForm] IFormFileCollection files, [FromForm] Guid articuloId)
    {
        try
        {
            var foto = await fotoService.Upload(files, articuloId);
            return Ok(foto);
        } catch (Exception e)
        {
            return this.Problem(e.StackTrace, title: "Ha ocurrido un error inesperado.");
        }
    }
    
    [HttpGet("{articuloId}")]
    public async Task<ActionResult> GetByArticulo(Guid articuloId)
    {
        try
        {
            var fotos = await fotoService.GetByArticulo(articuloId);
            return Ok(fotos);
        } catch (Exception e)
        {
            return this.Problem(e.StackTrace, title: "Ha ocurrido un error inesperado.");
        }
    }
}