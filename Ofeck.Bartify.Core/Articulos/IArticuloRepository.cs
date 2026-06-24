using Ofeck.Bartify.Core.Models;
using Ofeck.Bartify.Core.Articulos.DTOs;

namespace Ofeck.Bartify.Core.Articulos;

public interface IArticuloRepository
{
    public Task Create<Articulo>(Articulo articulo);
    public Task<bool> Update(Articulo articulo);
    public Task<bool> Delete(Guid id);
    public Task<List<GetArticuloDto>> GetAll();
    public Task<GetArticuloByIdDto> GetById(Guid id);
    public Task<List<GetArticuloDto>> GetByUserId(Guid userId);
}