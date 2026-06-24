using Microsoft.AspNetCore.Mvc;
using Ofeck.Bartify.Core.Ubicaciones;
using Ofeck.Bartify.Core.Models;

namespace Ofeck.Bartify.APIEndpoints.Controllers;

[ApiController]
[Route("api/ubicaciones")]
public class UbicacionController: ControllerBase
{
    private readonly UbicacionService ubicacionService;

    public UbicacionController(UbicacionService ubicacionService)
    {
        this.ubicacionService = ubicacionService;
    }

    [HttpGet]
    public async Task<ActionResult<List<string>>> GetUbicaciones()
    {
        try
        {
            var ubicaciones = await this.ubicacionService.GetUbicaciones();
            
            return this.Ok(ubicaciones);
        } catch(KeyNotFoundException k)
        {
            return this.NotFound(k.Message);
        } catch (Exception e)
        {
            return this.Problem(e.StackTrace, title: "Ha ocurrido un error inesperado.");
        }
    }
}