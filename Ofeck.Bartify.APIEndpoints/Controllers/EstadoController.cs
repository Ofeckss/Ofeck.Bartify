using Microsoft.AspNetCore.Mvc;
using Ofeck.Bartify.Core.Estados;
using Ofeck.Bartify.Core.Models;

namespace Ofeck.Bartify.APIEndpoints.Controllers;

[ApiController]
[Route("api/estados")]
public class EstadoController: ControllerBase
{
    private readonly EstadoService estadoService;

    public EstadoController(EstadoService estadoService)
    {
        this.estadoService = estadoService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Estado>>> GetEstados()
    {
        try
        {
            var estados = await this.estadoService.GetEstados();
            
            return this.Ok(estados);
        } catch(KeyNotFoundException k)
        {
            return this.NotFound(k.Message);
        } catch (Exception e)
        {
            return this.Problem(e.StackTrace, title: "Ha ocurrido un error inesperado.");
        }
    }
}