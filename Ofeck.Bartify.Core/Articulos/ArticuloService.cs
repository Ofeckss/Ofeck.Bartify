using System.Text.RegularExpressions;
using Ofeck.Bartify.Core.Articulos.DTOs;
using Ofeck.Bartify.Core.Models;
using Ofeck.Bartify.Core.Articulos.Requests;

namespace Ofeck.Bartify.Core.Articulos;

public class ArticuloService
{
    private readonly IArticuloRepository repository;
    public ArticuloService(IArticuloRepository repository)
    {
        this.repository = repository;
    }

    public async Task<Articulo> Create(CreateArticuloRequest request, Guid VendedorId)
    {
        var a = new Articulo(
            Guid.CreateVersion7(),
            request.Nombre,
            request.Descripcion,
            request.Precio,
            VendedorId,
            request.CategoriaId,
            request.EsTrueque,
            request.EstadoId,
            request.UbicacionId,
            true,
            DateTime.Now,
            DateTime.Now
        );
        
        await this.repository.Create(a);
        return a;
    }

    public async Task<List<GetArticuloDto>> GetAll()
    {
        return await this.repository.GetAll();
    }

    public async Task<GetArticuloByIdDto> GetById(Guid id)
    {
        return await this.repository.GetById(id);
    }

    public async Task<List<GetArticuloDto>> GetByUserId(Guid id)
    {
        return await this.repository.GetByUserId(id);
    }

    public async Task<List<GetArticuloDto>> GetFiltered(GetFilteredRequest request)
    {
        return await this.repository.GetFiltered(request);
    }

    public async Task Delete(Guid id)
    {
        var found = await this.repository.Delete(id);

        if (!found) 
            throw new KeyNotFoundException("Articulo no encontrado");
    }

    public async Task Update(UpdateArticuloRequest request, Guid Id)
    {
        var found = await this.repository.Update(request, Id);

        if (!found)
            throw new KeyNotFoundException("Articulo no encontrado");
    }
}