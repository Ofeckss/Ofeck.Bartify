using Microsoft.AspNetCore.Mvc;
using Ofeck.Bartify.Core.Categorias;
using Ofeck.Bartify.Core.Models;

namespace Ofeck.Bartify.APIEndpoints.Controllers;

[ApiController]
[Route("api/categorias")]
public class CategoriaController: ControllerBase
{
    private readonly CategoriaService categoriaService;

    public CategoriaController(CategoriaService categoriaService)
    {
        this.categoriaService = categoriaService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Categoria>>> GetParents()
    {
        try
        {
            var categorias = await this.categoriaService.GetParents();
            
            return this.Ok(categorias);
        } catch(KeyNotFoundException k)
        {
            return this.NotFound(k.Message);
        } catch (Exception e)
        {
            return this.Problem(e.StackTrace, title: "Ha ocurrido un error inesperado.");
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<List<Categoria>>> GetSons(int id)
    {
        try
        {
            var sons = await this.categoriaService.GetSons(id);
            
            return this.Ok(sons);
        } catch(KeyNotFoundException k)
        {
            return this.NotFound(k.Message);
        } catch (Exception e)
        {
            return this.Problem(e.StackTrace, title: "Ha ocurrido un error inesperado.");
        }
    }
}