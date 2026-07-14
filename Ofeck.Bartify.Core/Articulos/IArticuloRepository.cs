using Ofeck.Bartify.Core.Models;
using Ofeck.Bartify.Core.Articulos.DTOs;
using Ofeck.Bartify.Core.Articulos.Requests;

namespace Ofeck.Bartify.Core.Articulos;

public interface IArticuloRepository
{
    public Task Create<Articulo>(Articulo articulo);
    public Task<bool> Update(UpdateArticuloRequest request, Guid Id);
    public Task<bool> Delete(Guid Id);
    public Task<List<GetArticuloDto>> GetAll();
    public Task<GetArticuloByIdDto> GetById(Guid id);
    public Task<List<GetArticuloDto>> GetByUserId(Guid userId);
    public Task<List<GetArticuloDto>> GetFiltered(GetFilteredRequest request);
}